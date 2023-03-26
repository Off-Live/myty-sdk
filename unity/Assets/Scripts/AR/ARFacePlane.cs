using MYTYKit.MotionTemplates;
using UnityEngine;

public class ARFacePlane : MonoBehaviour
{
    [SerializeField]
    MotionTemplateMapper m_faceLocation;
    [SerializeField]
    MeshRenderer m_arBounds;
    
    [SerializeField]
    Transform m_facePlane;
    
    Vector3[] m_vertices;

    // Start is called before the first frame update
    void Start()
    {
        m_vertices = new Vector3[468];
    }

    // Update is called once per frame
    private void UpdateFromMotionTemplate()
    {
        var points = (m_faceLocation.GetTemplate("FacePoints") as PointsTemplate)!.points;
        if (points.Length < 468) return;
        var bounds = m_arBounds.bounds;
        var sumPosition = Vector3.zero;

        for (var i = 0; i < 468; i++)
        {
            m_vertices[i] = new Vector3(-(points[i].x + 0.5f) * bounds.size.x,
                (points[i].y + 0.5f) * bounds.size.y,
                points[i].z + 0.5f
            );
            sumPosition += m_vertices[i];
        }
        
        sumPosition /= 468;

        m_facePlane.localPosition = new Vector3(sumPosition.x, sumPosition.y, -1);

        var height = (m_vertices[10] - m_vertices[152]).magnitude;
        var width = (m_vertices[454] - m_vertices[234]).magnitude;
        var size = (width + height) / 2 * 0.3f;

        m_facePlane.localScale = new Vector3(size, size, size);
    }

    void Update()
    {
        UpdateFromMotionTemplate();
    }
}
