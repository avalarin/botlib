using System.Threading.Tasks;

namespace BotLib.Core.Utils {
    public static class TaskExtensions {

        public static Task RethrowExceptions(this Task task) {
            task.GetAwaiter().OnCompleted(task.Wait);
            return task;
        }
        
    }
}