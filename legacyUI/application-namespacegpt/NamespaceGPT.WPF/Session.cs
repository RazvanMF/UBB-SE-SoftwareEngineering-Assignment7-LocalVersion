using System.Windows.Controls;

namespace NamespaceGPT.WPF
{
    public class Session
    {
        public int UserId { get; set; } = 0;
        public Frame Frame { get; set; } = null!;
        private static readonly Session Instance = new ();

        public static Session GetInstance()
        {
            return Instance;
        }
    }
}
