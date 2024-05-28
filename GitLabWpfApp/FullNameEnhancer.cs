using GitLabWpfApp.ViewModels;

namespace GitLabWpfApp
{
    public class FullNameEnhancer
    {
        private readonly Stack<string> _path = new();

        public void EnchanceWithFullNames(IEnumerable<ProjectNodeViewModel> nodes)
        {
            string separator = @"\";
            string curpath = string.Join(separator, _path.Reverse().ToArray());

            foreach (ProjectNodeViewModel node in nodes)
            {
                string prefix = curpath == "" ? "" : $"{curpath}{separator}";
                node.FullName = $"{prefix}{node.DisplayName}";
                _path.Push(node.DisplayName);
                foreach (ProjectNodeViewModel child in node.Children)
                {
                    EnchanceWithFullNames(node.Children);
                }

                _path.Pop();
            }
        }
    }
}