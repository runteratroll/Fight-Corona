using UnityEngine.EventSystems; //오브젝트의 터치(상호작용)와 관련된 이름 공간
using UnityEngine;
using UnityEngine.UI;

//사용할 수 있는 인터페이스 아래등
public class VirtualController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Image imageBackground;
    private Image imageController;
    private Vector2 touchPosition;
    private void Awake()
    {
        imageBackground = GetComponent<Image>();
        imageController = transform.GetChild(0).GetComponent<Image>(); //첫번째 자식 오브젝트의 정보를 얻는다
    }
    //터치 시작 시 1회
    public void OnPointerDown(PointerEventData eventData) //IPointerDownHandler 인터페이스를 부모로 상속받을 경우 작성해야한다
    {
        //메소드가 1회 실행된다 이스크립트를 가지고 있는 
        //Debug.Log("Touch Begin : " + eventData);
        
    }
    //터치 상태일 때 매 프레임
    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;

        // 조이스틱의 위치가 어디에 있든 동일한 값을 연산하기 위해
        // touchPosition의 위치 값은 이미지의 현재 위치를 기준으로
        // 얼마나 떨어져 있는지에 따라 다르게 나온다
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            //rectTransform 앵커 및 피벗 정보.
            // touchPosition 값의 정규화 [0 ~ 1]
            // touchPosition을 이미지 크기로 나눔
            touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / imageBackground.rectTransform.sizeDelta.y);

            // touchPosition 값의 정규화 [-n ~ n]
            // 왼쪽(-1), 중심(0), 오른쪽(1)로 변경하기 위해 touchPosition.x*2 -1
            // 아래(-1), 중심(0), 위(1)로 변경하기 위해 touchPosition.y*2 -1
            // 이 수식은 Pivot에 따라 달라진다. (좌하단 Pivot 기준)
            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);

            // touchPosition 값의 정규화 [-1 ~ 1]
            // 가상 조이스틱 배경 이미지 밖으로 터치가 나가게 되면 -1 ~ 1보다 큰 값이 나올 수 있다
            // 이 때 normalized를 이용해 -1 ~ 1사이의 값으로 정규화
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            // 가상 조이스틱 컨트롤러 이미지 이동
            // 컨트롤러가 배경 이미지 바깥으로 튀어나가게 하고 싶지 않으면 나눠주는 값을 더 크게 설정해야 한다
            imageController.rectTransform.anchoredPosition = new Vector2(
                touchPosition.x * imageBackground.rectTransform.sizeDelta.x / 2,
                touchPosition.y * imageBackground.rectTransform.sizeDelta.y / 2);
            //Debug.Log("Touch & Drag : " + eventData);
        }
    
    }

    // 터치 종료 시 1회
    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치 종료 시 이미지의 위치를 중앙으로 다시 옮긴다.
        imageController.rectTransform.anchoredPosition = Vector2.zero;
        // (플레이어)다른 오브젝트에서 이동 방향으로 사용하기 때문에 이동 방향도 초기화
        touchPosition = Vector2.zero;
        // 컨트롤러를 이용하지 않을 때 플레이어오브젝트도 이동하기 않게 초기화
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
