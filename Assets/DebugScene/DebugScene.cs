using System.Collections;
using System.Collections.Generic;
using System.Text;
using QuickEye.ImportableAssets;
using QuickEye.ImportableAssets.Samples.JsonImporter;
using UnityEngine;
using UnityEngine.UI;

public class DebugScene : MonoBehaviour
{
    [SerializeField]
    private Text debugLabel;

    [SerializeField]
    private ServerConfig serverConfig;
    
    void Start()
    {
        var serializer = new JsonSerializer();
        debugLabel.text = serializer.ToText(serverConfig);
    }

}
