using UnityEngine.EventSystems; //������Ʈ�� ��ġ(��ȣ�ۿ�)�� ���õ� �̸� ����
using UnityEngine;
using UnityEngine.UI;

//����� �� �ִ� �������̽� �Ʒ���
public class VirtualController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Image imageBackground;
    private Image imageController;
    private Vector2 touchPosition;
    private void Awake()
    {
        imageBackground = GetComponent<Image>();
        imageController = transform.GetChild(0).GetComponent<Image>(); //ù��° �ڽ� ������Ʈ�� ������ ��´�
    }
    //��ġ ���� �� 1ȸ
    public void OnPointerDown(PointerEventData eventData) //IPointerDownHandler �������̽��� �θ�� ��ӹ��� ��� �ۼ��ؾ��Ѵ�
    {
        //�޼ҵ尡 1ȸ ����ȴ� �̽�ũ��Ʈ�� ������ �ִ� 
        //Debug.Log("Touch Begin : " + eventData);
        
    }
    //��ġ ������ �� �� ������
    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;

        // ���̽�ƽ�� ��ġ�� ��� �ֵ� ������ ���� �����ϱ� ����
        // touchPosition�� ��ġ ���� �̹����� ���� ��ġ�� ��������
        // �󸶳� ������ �ִ����� ���� �ٸ��� ���´�
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            //rectTransform ��Ŀ �� �ǹ� ����.
            // touchPosition ���� ����ȭ [0 ~ 1]
            // touchPosition�� �̹��� ũ��� ����
            touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / imageBackground.rectTransform.sizeDelta.y);

            // touchPosition ���� ����ȭ [-n ~ n]
            // ����(-1), �߽�(0), ������(1)�� �����ϱ� ���� touchPosition.x*2 -1
            // �Ʒ�(-1), �߽�(0), ��(1)�� �����ϱ� ���� touchPosition.y*2 -1
            // �� ������ Pivot�� ���� �޶�����. (���ϴ� Pivot ����)
            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);

            // touchPosition ���� ����ȭ [-1 ~ 1]
            // ���� ���̽�ƽ ��� �̹��� ������ ��ġ�� ������ �Ǹ� -1 ~ 1���� ū ���� ���� �� �ִ�
            // �� �� normalized�� �̿��� -1 ~ 1������ ������ ����ȭ
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            // ���� ���̽�ƽ ��Ʈ�ѷ� �̹��� �̵�
            // ��Ʈ�ѷ��� ��� �̹��� �ٱ����� Ƣ����� �ϰ� ���� ������ �����ִ� ���� �� ũ�� �����ؾ� �Ѵ�
            imageController.rectTransform.anchoredPosition = new Vector2(
                touchPosition.x * imageBackground.rectTransform.sizeDelta.x / 2,
                touchPosition.y * imageBackground.rectTransform.sizeDelta.y / 2);
            //Debug.Log("Touch & Drag : " + eventData);
        }
    
    }

    // ��ġ ���� �� 1ȸ
    public void OnPointerUp(PointerEventData eventData)
    {
        // ��ġ ���� �� �̹����� ��ġ�� �߾����� �ٽ� �ű��.
        imageController.rectTransform.anchoredPosition = Vector2.zero;
        // (�÷��̾�)�ٸ� ������Ʈ���� �̵� �������� ����ϱ� ������ �̵� ���⵵ �ʱ�ȭ
        touchPosition = Vector2.zero;
        // ��Ʈ�ѷ��� �̿����� ���� �� �÷��̾������Ʈ�� �̵��ϱ� �ʰ� �ʱ�ȭ
    }

    public float Horizontal()
    {
        return touchPosition.x;
    }

    public float Vertical()
    {
        return touchPosition.y;
    }

}
