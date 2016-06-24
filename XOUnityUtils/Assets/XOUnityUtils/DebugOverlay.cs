using UnityEngine;
using System.Linq;

public class DebugOverlay : XMonoBehaviour {
    
    [SerializeField] private bool m_Extended = false;

    const int k_FrameDeltaCount = 64;
    float[] m_FrameDeltas = new float[k_FrameDeltaCount];
    int m_FrameDeltaIndex = 0;

    float m_AverageDeltaTime = 0f;
    float m_AverageFPS = 0f;
    
	protected override void XSetup(XSetupKind kind) {
	   for(int i = 0; i < k_FrameDeltaCount; ++i) {
           m_FrameDeltas[i] = -1f;
       }
	}
	
	protected override void XUpdate(XUpdateKind kind) {
	   m_FrameDeltas[m_FrameDeltaIndex++] = Time.deltaTime;
       if(m_FrameDeltaIndex >= k_FrameDeltaCount) {
           m_FrameDeltaIndex = 0;
       }
       m_AverageDeltaTime = m_FrameDeltas.Average(f =>{ return Mathf.Max(0f, f); });
       m_AverageFPS = 1f / m_AverageDeltaTime;
	}
    
    private void ShadowLabel(Rect r, string s) {
        const float distance = 1f;
        GUI.color = Color.black;
        GUI.Label(r, s);
        GUI.color = Color.white;
        r.x -= distance;
        r.y -= distance;
        GUI.Label(r, s);
    }
    
    private void OnGUI() {
        const float em = 19f;
        ShadowLabel(new Rect(em, em*1f, 200, em), "av fps: " + m_AverageFPS.ToString("0.00"));
        ShadowLabel(new Rect(em, em*2f, 200, em), "av mspf: " + (m_AverageDeltaTime*1000f).ToString("0.00"));
        if(m_Extended) {
            ShadowLabel(new Rect(em, em*3f, 200, em), "av dt: " + m_AverageDeltaTime.ToString("0.00"));
            
            ShadowLabel(new Rect(em, em*4f, 200, em), "fps: " + (1f/m_FrameDeltas[m_FrameDeltaIndex]).ToString("0.00"));
            ShadowLabel(new Rect(em, em*5f, 200, em), "mspf: " + (m_FrameDeltas[m_FrameDeltaIndex]*1000f).ToString("0.00"));
            ShadowLabel(new Rect(em, em*6f, 200, em), "dt: " + m_FrameDeltas[m_FrameDeltaIndex].ToString("0.00"));
            
        }
    }
}
