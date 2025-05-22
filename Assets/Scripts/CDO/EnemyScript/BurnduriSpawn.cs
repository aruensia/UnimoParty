using UnityEngine;

public class BurnduriSpawn : MonoBehaviour
{
    public EnemySpawnerCommand enemySpawnerCommand;
    public GameObject testPrefab;
    Vector3 spawnPosition;

    [SerializeField] Transform player;
    [SerializeField] Transform donutTransform;
    [SerializeField] float distanceGap; 
    float angle; // ����
    int r = 15; //������

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("SpawnEnemy", 1f, 2f);
    }


    void SpawnEnemy()
    {

        //enemySpawnerCommand.SpawnEnemy("Burnduri", isPlayerHere(), 5);
        enemySpawnerCommand.SpawnEnemy("Burnduri", IsDoWhile(), 5);
        enemySpawnerCommand.SpawnEnemy("Burnduri", IsDoWhile(), 5);
        enemySpawnerCommand.SpawnEnemy("Burnduri", IsDoWhile(), 5);
        enemySpawnerCommand.SpawnEnemy("Burnduri", IsDoWhile(), 5);
        enemySpawnerCommand.SpawnEnemy("Burnduri", IsDoWhile(), 5);

    }

    Vector3 DonutPostion()
    {
        angle = UnityEngine.Random.Range(0, 360);
        float x = Mathf.Cos(angle) * r;
        float y = donutTransform.transform.position.y;
        float z = Mathf.Sin(angle) * r;
        donutTransform.position = new Vector3(x, y, z);

        return donutTransform.position;
    }

    Vector3 isPlayerHere()
    {
        for (int i = 0; i < 5; i++)
        {
            //var dd = donutTransform.position; //�׽�Ʈ����
            var dd = DonutPostion(); //����

            if (Vector3.Distance(dd, player.position) >= distanceGap)
            {
                Debug.Log("�÷��̾ ��ó �ƴԤ�");
                return dd;
            }
            else
            {
                Debug.Log("�÷��̾ ��ó��");
            }
        }
        //���߿� player��ó �ƴҶ����� ������ɵ�
        Debug.Log("i�� ���Ǵµ� �÷��̾ ��ó��");

        var aa = DonutPostion();
        return aa;
    }


    Vector3 IsDoWhile()
    {
        Vector3 dd;
        int roof = 0; // loop
        do 
        { dd = DonutPostion();roof++; } 
        while ((Vector3.Distance(dd, player.position) >= distanceGap)&&roof>=10);

        if (roof >= 10) { Debug.Log("���ѷ����ɻ�"); }

        return dd;
    }







}
