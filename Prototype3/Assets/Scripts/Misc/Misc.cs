using UnityEngine.SceneManagement;

namespace Misc
{
    public delegate void VoidStrategy();

    public static class StandardMethods
    {
        public static void None() { }

        public static void SceneSwitch(int sceneIndex) {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
