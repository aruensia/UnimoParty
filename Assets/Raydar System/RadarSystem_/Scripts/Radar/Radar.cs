using Ilumisoft.RadarSystem.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Ilumisoft.RadarSystem
{
    /// <summary>
    /// Radar �ý���
    /// 
    /// �� Ŭ������ ������ Locatable ������Ʈ���� ���̴� UI�� ������ ���·� ǥ���ϴ� ������ �Ѵ�.
    /// - Player�� �������� �Ÿ� �� ������ ����Ͽ� ������ ��ġ�� ����
    /// - ȸ���� Ȱ��ȭ�� ���, RotatingContainer�� ȸ���� ���� ��ü �����ܵ��� ȸ��
    /// - iconContainer�� �������� ���� �� ����
    /// </summary>
    [AddComponentMenu("Radar System/Radar")]
    [DefaultExecutionOrder(-10)]
    public class Radar : MonoBehaviour
    {
        /// <summary>
        /// ���� ��� ������Ʈ�� �ش� UI �������� �����ϴ� ����
        /// </summary>
        private readonly Dictionary<LocatableComponent, LocatableIconComponent> locatableIconDictionary = new();

        [Header("UI ����")]
        [SerializeField]
        [Tooltip("�������� ��ġ�� �����̳� (RotatingContainer)")]
        private RectTransform iconContainer;

        [SerializeField]
        [Tooltip("ȸ���� ����� ������Ʈ (RotatingContainer)")]
        private Transform rotatingRoot;

        [Header("����")]
        [SerializeField, Min(1)]
        [Tooltip("���̴� ���� �Ÿ� ���� (����: ����)")]
        private float range = 20;

        [SerializeField]
        [Tooltip("�÷��̾� ȸ���� ���̴� ȸ���� �ݿ����� ����")]
        private bool applyRotation = true;

        [Tooltip("���̴��� ������ �Ǵ� �÷��̾� ������Ʈ")]
        public GameObject Player;
        private float radarUISize;

        /// <summary>
        /// ���� �Ÿ� ����
        /// </summary>
        public float Range { get => range; set => range = value; }

        /// <summary>
        /// ȸ�� �ݿ� ���� ����
        /// </summary>
        public bool ApplyRotation { get => applyRotation; set => applyRotation = value; }

        private void OnEnable()
        {
            LocatableManager.OnLocatableAdded += OnLocatableAdded;
            LocatableManager.OnLocatableRemoved += OnLocatableRemoved;
        }

        private void OnDisable()
        {
            LocatableManager.OnLocatableAdded -= OnLocatableAdded;
            LocatableManager.OnLocatableRemoved -= OnLocatableRemoved;
        }

        /// <summary>
        /// ���ο� Locatable�� �߰��Ǿ��� �� ������ ���� �� ���
        /// </summary>
        private void OnLocatableAdded(LocatableComponent locatable)
        {
            if (locatable != null && !locatableIconDictionary.ContainsKey(locatable))
            {
                var icon = locatable.CreateIcon();
                icon.transform.SetParent(iconContainer.transform, false);
                locatableIconDictionary.Add(locatable, icon);
            }
        }

        /// <summary>
        /// Locatable�� ���ŵǾ��� �� ������ ���� �� ��� ����
        /// </summary>
        private void OnLocatableRemoved(LocatableComponent locatable)
        {
            if (locatable != null && locatableIconDictionary.TryGetValue(locatable, out LocatableIconComponent icon))
            {
                locatableIconDictionary.Remove(locatable);
                Destroy(icon.gameObject);
            }
        }

        /// <summary>
        /// �� �����Ӹ��� ������ ��ġ ������Ʈ �� ȸ�� ����
        /// </summary>
        private void Update()
        {
            if (Player != null)
            {
                UpdateRotation();
                UpdateLocatableIcons();
            }
        }

        /// <summary>
        /// �ܺο��� ������ ���̴� ȸ���� ������ �� ȣ��
        /// </summary>
        public void RefreshRotationImmediately()
        {
            UpdateRotation();
        }
        ///
        ///
        ///


        /// <summary>
        /// �÷��̾� ȸ���� ���� rotatingRoot�� ȸ�� ����
        /// </summary>
        private void UpdateRotation()
        {
            if (applyRotation && rotatingRoot != null)
            {
                Vector3 forward = Vector3.ProjectOnPlane(Player.transform.forward, Vector3.up);

                if (forward.sqrMagnitude > 0.001f)
                {
                    Quaternion rotation = Quaternion.LookRotation(forward);
                    float zRotation = -rotation.eulerAngles.y;
                    rotatingRoot.localRotation = Quaternion.Euler(0, 0, zRotation);
                }
            }
        }

        /// <summary>
        /// �� �������� ��ġ�� ����Ͽ� ȭ�鿡 ��ġ
        /// </summary>
        private void UpdateLocatableIcons()
        {
            foreach (var locatable in locatableIconDictionary.Keys)
            {
                if (locatableIconDictionary.TryGetValue(locatable, out var icon))
                {
                    if (TryGetIconLocation(locatable, out var iconLocation))
                    {
                        icon.SetVisible(true);
                        icon.GetComponent<RectTransform>().anchoredPosition = iconLocation;
                    }
                    else
                    {
                        icon.SetVisible(false);
                    }
                }
            }
        }

        /// <summary>
        /// Ư�� Locatable�� UI �� ��ġ ���
        /// - ȸ���� ȸ�� �����̳ʿ��� ó���ϹǷ� ��� �ÿ� �ݿ����� ����
        /// </summary>
        private bool TryGetIconLocation(LocatableComponent locatable, out Vector2 iconLocation)
        {
            iconLocation = GetDistanceToPlayer(locatable);

            float radarSize = GetRadarUISize();
            float scale = radarSize / range;

            iconLocation *= scale;

            //  �ʹ� ������ �ּ� ������ ���� ���� (�߽� ���� ����)
            float minRadius = radarUISize * 0.25f; // �ּ� �Ÿ� UI ���� (������ ���� ����)

            if (iconLocation.magnitude < minRadius)
            {
                iconLocation = iconLocation.normalized * minRadius;


            }



            // ���� ���� �ȿ� �ִ� ��츸 ��ġ ����
            if (iconLocation.sqrMagnitude < radarSize * radarSize || locatable.ClampOnRadar)
            {
                iconLocation = Vector2.ClampMagnitude(iconLocation, radarSize);
                return true;
            }

            return false;
        }

        /// <summary>
        /// ���̴� UI�� ������(�ȼ� ����) ��ȯ
        /// </summary>
        private float GetRadarUISize()
        {
            return iconContainer.rect.width / 2;
        }

        /// <summary>
        /// �÷��̾� ���� ����� ��� �Ÿ� ��� (X-Z ���)
        /// </summary>
        private Vector2 GetDistanceToPlayer(LocatableComponent locatable)
        {
            Vector3 distance = locatable.transform.position - Player.transform.position;
            return new Vector2(distance.x, distance.z);
        }
    }
}

