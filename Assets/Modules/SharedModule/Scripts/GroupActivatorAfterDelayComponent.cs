
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Modules.SharedModule.Scripts
{
    public class GroupActivatorAfterDelayComponent : MonoBehaviour
    {
        [SerializeField] private float _incrementTimeTimeBeforeActivate;
        [SerializeField] private float _startTimeBeforeActivate;
        
        [ContextMenu("Appoint new time before activate time")]
        public void AppointNewTimeBeforeActivate()
        {
            var children = GetComponentsInChildren<ActivatorAfterDelayComponent>(true);
            for (var i = 0; i < children.Length; i++)
            {
                var activatorAfterDelayComponent = children[i];

                // 1. Записываем состояние объекта перед изменением (для работы Ctrl+Z)
#if UNITY_EDITOR
                Undo.RecordObject(activatorAfterDelayComponent, "Set Time Before Activate");
#endif

                // Меняем значение
                activatorAfterDelayComponent.SetTimeBeforeActivate(_startTimeBeforeActivate + _incrementTimeTimeBeforeActivate * i);

                // 2. Помечаем объект как измененный, чтобы Unity записал его на диск
#if UNITY_EDITOR
                EditorUtility.SetDirty(activatorAfterDelayComponent);
#endif
            }

            // 3. Помечаем саму сцену как измененную (появится звездочка * возле названия сцены)
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
#endif
        }
    }
}