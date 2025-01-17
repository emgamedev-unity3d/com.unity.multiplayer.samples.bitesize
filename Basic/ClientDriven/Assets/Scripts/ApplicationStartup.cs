using Unity.Services.Core;
using UnityEngine;

public class ApplicationStartup : MonoBehaviour
{
    [SerializeField]
    private GameObject m_clientManager;

    [SerializeField]
    private GameObject m_serverManager;

    private async void Start()
    {
        ApplicationStartupData.InitializeApplicationData();

        bool isServerBuild =
            SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null;

        await UnityServices.InitializeAsync();

        if (isServerBuild)
        {
            Debug.Log($"Starting server build, no graphics device present");

            m_clientManager.SetActive(false);

            m_serverManager.TryGetComponent(out ServerSingleton serverSingleton);

            await serverSingleton.StartGameServerAsync();

            return;
        }

        Debug.Log(
            @$"Starting client build, Graphics device type is {SystemInfo.graphicsDeviceType}");

        m_serverManager.SetActive(false);

        m_clientManager.TryGetComponent(out ClientSingleton clientSingleton);

        await clientSingleton.StartGameClientAsync();
    }
}