//using Ilumisoft.RadarSystem.UI;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Ilumisoft.RadarSystem
//{
//    /// <summary>
//    /// ���̴� �ý����� �ٽ� Ŭ����
//    /// - ������ ������Ʈ(Locatable)�� UI �󿡼� ���������� ǥ��
//    /// - �÷��̾ �������� �������� ���̴� ���� ��ġ
//    /// - �÷��̾��� ȸ���� ���� ȸ�� ���� ����
//    /// </summary>
//    [AddComponentMenu("Radar System/Radar")]
//    [DefaultExecutionOrder(-10)]
//    public class Radar : MonoBehaviour
//    {
//        /// <summary>
//        /// �� Locatable ������Ʈ�� �ش� ������ UI ������Ʈ�� �����ϴ� Dictionary
//        /// </summary>
//        readonly Dictionary<LocatableComponent, LocatableIconComponent> locatableIconDictionary = new();

//        [SerializeField]
//        [Tooltip("�������� ���� UI �����̳� (���̴� ���� ����)")]
//        private RectTransform iconContainer;

//        [SerializeField, Min(1)]
//        [Tooltip("���̴� ���� ���� (����: ����)")]
//        private float range = 20;

//        [SerializeField]
//        [Tooltip("�÷��̾� ȸ������ ���̴��� �������� ����")]
//        private bool applyRotation = true;

//        /// <summary>
//        /// ���� ���� ������Ƽ
//        /// </summary>
//        public float Range { get => range; set => range = value; }

//        /// <summary>
//        /// ȸ�� ���� ���� ������Ƽ
//        /// </summary>
//        public bool ApplyRotation { get => applyRotation; set => applyRotation = value; }

//        /// <summary>
//        /// �÷��̾� ���� (���̴� ������)
//        /// </summary>
//        public GameObject Player;

//        private void OnEnable()
//        {
//            // Locatable ������Ʈ�� �߰�/���ŵ� �� �̺�Ʈ ����
//            LocatableManager.OnLocatableAdded += OnLocatableAdded;
//            LocatableManager.OnLocatableRemoved += OnLocatableRemoved;
//        }

//        private void OnDisable()
//        {
//            // �̺�Ʈ ����
//            LocatableManager.OnLocatableAdded -= OnLocatableAdded;
//            LocatableManager.OnLocatableRemoved -= OnLocatableRemoved;
//        }

//        /// <summary>
//        /// Locatable ������Ʈ�� �߰��Ǿ��� �� ȣ���
//        /// - �������� �����Ͽ� Dictionary�� ����
//        /// </summary>
//        private void OnLocatableAdded(LocatableComponent locatable)
//        {
//            if (locatable != null && !locatableIconDictionary.ContainsKey(locatable))
//            {
//                var icon = locatable.CreateIcon();
//                icon.transform.SetParent(iconContainer.transform, false);
//                locatableIconDictionary.Add(locatable, icon);
//            }
//        }

//        /// <summary>
//        /// Locatable ������Ʈ�� ���ŵǾ��� �� ȣ���
//        /// - ������ ���� �� Dictionary���� ����
//        /// </summary>
//        private void OnLocatableRemoved(LocatableComponent locatable)
//        {
//            if (locatable != null && locatableIconDictionary.TryGetValue(locatable, out LocatableIconComponent icon))
//            {
//                locatableIconDictionary.Remove(locatable);
//                Destroy(icon.gameObject);
//            }
//        }

//        private void Update()
//        {
//            if (Player != null)
//            {
//                UpdateLocatableIcons();
//            }
//        }

//        /// <summary>
//        /// ��� �������� ��ġ�� �� �����Ӹ��� ������Ʈ
//        /// </summary>
//        private void UpdateLocatableIcons()
//        {
//            foreach (var locatable in locatableIconDictionary.Keys)
//            {
//                if (locatableIconDictionary.TryGetValue(locatable, out var icon))
//                {
//                    if (TryGetIconLocation(locatable, out var iconLocation))
//                    {
//                        icon.SetVisible(true);
//                        icon.GetComponent<RectTransform>().anchoredPosition = iconLocation;
//                    }
//                    else
//                    {
//                        icon.SetVisible(false);
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// �������� ���̴� UI �� ��� ��ġ���� ���
//        /// - ȭ�� ���� ��� ��ġ ��ȯ
//        /// - ȭ�� ���� ��� false ��ȯ
//        /// </summary>
//        private bool TryGetIconLocation(LocatableComponent locatable, out Vector2 iconLocation)
//        {
//            // �÷��̾�κ��� ��� ��ġ ��� (x, z ��� ����)
//            iconLocation = GetDistanceToPlayer(locatable);

//            float radarSize = GetRadarUISize();
//            float scale = radarSize / Range;

//            iconLocation *= scale;

//            // ȸ�� ���� ���ο� ���� ���� ȸ��
//            if (ApplyRotation)
//            {
//                var forward = Vector3.ProjectOnPlane(Player.transform.forward, Vector3.up);
//                var rotation = Quaternion.LookRotation(forward);
//                var euler = rotation.eulerAngles;

//                // �¿� ���� (UI ���� ���� ȸ�� ���� ����)
//                euler.y = -euler.y;
//                rotation.eulerAngles = euler;

//                var rotated = rotation * new Vector3(iconLocation.x, 0.0f, iconLocation.y);
//                iconLocation = new Vector2(rotated.x, rotated.z);
//            }

//            // ���� ���� ��� Clamp ó�� �� ��ġ ��ȯ
//            if (iconLocation.sqrMagnitude < radarSize * radarSize || locatable.ClampOnRadar)
//            {
//                iconLocation = Vector2.ClampMagnitude(iconLocation, radarSize);
//                return true;
//            }

//            return false;
//        }

//        /// <summary>
//        /// ���̴� UI ������ ���ϱ�
//        /// </summary>
//        private float GetRadarUISize()
//        {
//            return iconContainer.rect.width / 2;
//        }

//        /// <summary>
//        /// �÷��̾�κ��� �������� �Ÿ� (x, z �� ����)
//        /// </summary>
//        private Vector2 GetDistanceToPlayer(LocatableComponent locatable)
//        {
//            Vector3 distance = locatable.transform.position - Player.transform.position;
//            return new Vector2(distance.x, distance.z);
//        }
//    }
//}

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
        [Tooltip("�������� ��ġ�� �����̳� (�Ϲ������� RotatingContainer)")]
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

