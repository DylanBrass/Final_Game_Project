using Unity.Netcode;
using UnityEngine;

public class hitBall : NetworkBehaviour
{
    [SerializeField]
    private float maxDragLength = 0.1f;


    private bool isMoving;

    private Rigidbody2D rb2d;

    LineRenderer lineRenderer;

    [SerializeField]
    private Material lineMaterial;



    private void Start()
    {

        rb2d = gameObject.GetComponent<Rigidbody2D>();

        if (IsLocalPlayer)
        {
            lineRenderer = this.gameObject.AddComponent<LineRenderer>();

            //This does not work in the build version: I am guessing it's because it 
            //does not have acess to the Shaders library
            //lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

            lineRenderer.material = new Material(lineMaterial);
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = new Color(255, 2255, 255, 0.25f);
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.renderingLayerMask = 3;
        }


    }

    //put as update
    private void Update()
    {

        if (IsLocalPlayer)
        {
            if (this.gameObject != null && Camera.main != null)
            {
                Vector2 directionOfHit = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.gameObject.transform.position;

                float strengthOfHit = Mathf.Clamp(Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)), 0, maxDragLength);
                Vector2 endPos = (Vector2)transform.position + (directionOfHit.normalized * strengthOfHit);

                if (rb2d.velocity.magnitude < 0.05f)
                {
                    isMoving = false;
                    gameObject.GetComponent<Collider2D>().isTrigger = true;
                }
                else
                {
                    isMoving = true;
                    gameObject.GetComponent<Collider2D>().isTrigger = false;
                }


                if (!isMoving)
                {


                    lineRenderer.SetPosition(1, endPos);


                    lineRenderer.SetPosition(0, this.gameObject.transform.position);


                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                    {
                        float force = (Vector2.Distance(this.gameObject.transform.position, endPos) * 100 / maxDragLength);

                        rb2d.AddForce(5 * force * -(endPos - (Vector2)transform.position).normalized);
                        GlobalVariables.numOfHitsForLvl++;
                        Debug.Log(GlobalVariables.numOfHitsForLvl);
                    }
                }



            }
        }
        if (isMoving)
            gameObject.GetComponent<Collider2D>().isTrigger = false;

    }


}
