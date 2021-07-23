using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public int speed;

    Rigidbody2D rigid;
    Animator anim;
    GameObject scanObj;
    Vector3 dirVec;
    float h;
    float v;
    bool isHorizontMove;

    //Mobile Key Var
    int up_Value;
    int down_Value;
    int left_Value;
    int right_Value;
    bool up_Down;
    bool down_Down;
    bool left_Down;
    bool right_Down;
    bool up_Up;
    bool down_Up;
    bool left_Up;
    bool right_Up;

    private void Awake() {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        Vector2 moveVec = isHorizontMove ? new Vector2(h, 0) : new Vector2(0, v);
        rigid.velocity = moveVec * speed;

        //Debug.DrawRay(rigid.position, dirVec * 0.7f, Color.green);
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));
        if (rayHit.collider != null) {
            scanObj = rayHit.collider.gameObject;
        }
        else {
            scanObj = null;
        }
    }

    private void Update() {
        h = gameManager.isAction ? 0 : Input.GetAxisRaw("Horizontal") + right_Value + left_Value;
        v = gameManager.isAction ? 0 : Input.GetAxisRaw("Vertical") + up_Value + down_Value;

        bool hDown = gameManager.isAction ? false : Input.GetButtonDown("Horizontal") || right_Down || left_Down;
        bool vDown = gameManager.isAction ? false : Input.GetButtonDown("Vertical") || up_Down || down_Down;
        bool hUp = gameManager.isAction ? false : Input.GetButtonUp("Horizontal") || right_Up || left_Up;
        bool vUp = gameManager.isAction ? false : Input.GetButtonUp("Vertical") || up_Up || down_Up;

        if (hDown) {
            isHorizontMove = true;
        }
        else if (vDown) {
            isHorizontMove = false;
        }
        else if (hUp || vUp) {
            isHorizontMove = h != 0;
        }

        //Animation
        if (anim.GetInteger("vAxisRaw") != v) {
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int)v);
        }
        else if (anim.GetInteger("hAxisRaw") != h) {
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int)h);
        }
        else {
            anim.SetBool("isChange", false);
        }

        //Looking Direction
        if (vDown && v == 1) {
            dirVec = Vector3.up;
        }
        else if (vDown && v == -1) {
            dirVec = Vector3.down;
        }
        else if (hDown && h == 1) {
            dirVec = Vector3.right;
        }
        else if (hDown && h == -1) {
            dirVec = Vector3.left;
        }

        if (Input.GetButtonDown("Jump") && scanObj != null) {
            gameManager.Action(scanObj);
        }

        //Mobile Var Init
        up_Down = false;
        down_Down = false;
        left_Down = false;
        right_Down = false;
        up_Up = false;
        down_Up = false;
        left_Up = false;
        right_Up = false;
    }

    public void ButtonDown(string type) {
        switch (type) {
            case "U":
                up_Value = 1;
                up_Down = true;
                break;
            case "D":
                down_Value = -1;
                down_Down = true;
                break;
            case "L":
                left_Value = -1;
                left_Down = true;
                break;
            case "R":
                right_Value = 1;
                right_Down = true;
                break;
            case "A":
                if (scanObj != null) {
                    gameManager.Action(scanObj);
                }
                break;
            case "C":
                gameManager.SubMenuActive();
                break;
        }
    }

    public void ButtonUp(string type) {
        switch (type) {
            case "U":
                up_Value = 0;
                up_Up = true;
                break;
            case "D":
                down_Value = 0;
                down_Up = true;
                break;
            case "L":
                left_Value = 0;
                left_Up = true;
                break;
            case "R":
                right_Value = 0;
                right_Up = true;
                break;
        }
    }
}
