using UnityEngine;

namespace Ilumisoft.RadarSystem
{
    /// <summary>
    /// ���̴��� ǥ�õǴ� ������ (����/ä����/�÷��̾� ��)
    /// </summary>
    public class RadarIcon : MonoBehaviour
    {
        [SerializeField]
        private GameObject graphic; // ������ �׷��� ������Ʈ (���� ó����)

        /// <summary>
        /// �������� �Ѱų� ����
        /// </summary>
        /// <param name="visible">���̰� ���� ����
        public void SetVisible(bool visible)
        {
            if (graphic != null)
                graphic.SetActive(visible);
            else
                gameObject.SetActive(visible); // �׷����� ������ ��ü on/off
        }
    }
}
