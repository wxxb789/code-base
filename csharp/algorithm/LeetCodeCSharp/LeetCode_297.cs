using System.Collections.Generic;
using System.Text;
using LeetCodeCSharp.DataStructure;

namespace LeetCodeCSharp
{
    // public class Codec
    public class LeetCode_297
    {
        // Encodes a tree to a single string.
        public string serialize(TreeNode root)
        {
            if (root == null) return "";
            var result = new StringBuilder();
            
            // preorder traversal
            PreOrder(root, result);
            
            return result.ToString();
        }

        // Decodes your encoded data to tree.
        public TreeNode deserialize(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;

            string[] nodes = data.Split(',');
            Queue<string> queue = new Queue<string>(nodes);

            return PreOrder(queue);
        }

        // preorder traversal for serialize, assume sb is non-null
        private void PreOrder(TreeNode root, StringBuilder sb)
        {
            if (root == null)
            {
                sb.Append("#,");
                return;
            }

            sb.Append($"{root.val},");
            PreOrder(root.left, sb);
            PreOrder(root.right, sb);
        }

        // preorder traversal for deserialize
        private TreeNode PreOrder(Queue<string> queue)
        {
            // check isEmpty if queue is not valid
            // terminate condition
            string value = queue.Dequeue();
            if (value.Equals("#")) return null;

            // TryParse if queue is not valid
            var head = new TreeNode(int.Parse(value));
            head.left = PreOrder(queue);
            head.right = PreOrder(queue);
            return head;
        }
    }
}