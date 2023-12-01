using UnityEngine;

public class ShooterIA : IAController
{
    public GameObject balaPrefab;
   

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Vector3 myVelocity = base.myVelocity;
        myRB.velocity = myVelocity;
        if (hasDetected)
        {
            Disparar();
            myVelocity.x = 0;
        }

        myRB.velocity = myVelocity;
    }
        

    private void Disparar()
    {
        if (Time.time > tiempoActual)
        {
            GameObject bala = Instantiate(balaPrefab, transform.position, Quaternion.identity);
            bala.GetComponent<Rigidbody2D>().velocity = new Vector2(direccion * 5.0f, 0f);

            //Frecuencia de disparo.
            tiempoActual = Time.time + 1.0f;
        }
    }
}