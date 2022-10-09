using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures
{
    public class BinaryTree
    {
    }

    public class TreeNode
    {
        public TreeNode Left  { get; set; }
        public TreeNode Right { get; set; }
        public int      Value { get; set; }
    }

    // Recursive Traversal
    public class BinaryTreeRecursiveTraversal
    {
        // 递归顺序，每个节点会被扫过3次
        public static void RecursiveOrder(TreeNode head)
        {
            if (head == null)
            {
                return;
            }

            // 1
            RecursiveOrder(head.Left);
            // 2
            RecursiveOrder(head.Right);
            // 3
        }

        public static void PreOrder(TreeNode head)
        {
            if (head == null)
            {
                return;
            }

            Console.WriteLine(head.Value);
            PreOrder(head.Left);
            PreOrder(head.Right);
        }

        public static void InOrder(TreeNode head)
        {
            if (head == null)
            {
                return;
            }

            InOrder(head.Left);
            Console.WriteLine(head.Value);
            InOrder(head.Right);
        }

        public static void PostOrder(TreeNode head)
        {
            if (head == null)
            {
                return;
            }

            PostOrder(head.Left);
            PostOrder(head.Right);
            Console.WriteLine(head.Value);
        }
    }

    // UnRecursive Traversal
    public class BinaryTreeUnRecursiveTraversal
    {
        // 遍历顺序：头左右
        // 入栈顺序：头右左
        public static void PreOrderUnRecursive(TreeNode head)
        {
            if (head != null)
            {
                Stack<TreeNode> stack = new Stack<TreeNode>();
                stack.Push(head);
                while (stack.Count > 0)
                {
                    head = stack.Pop();
                    Console.Write($"{head.Value}\t");
                    if (head.Right != null)
                    {
                        stack.Push(head.Right);
                    }

                    if (head.Left != null)
                    {
                        stack.Push(head.Left);
                    }
                }
            }

            Console.WriteLine("End");
        }

        // 遍历顺序：左头右
        // 入栈顺序：头左，直到为null，再处理右
        // 相当于用左边界构建整棵树
        public static void InOrderUnRecursive(TreeNode head)
        {
            if (head != null)
            {
                Stack<TreeNode> stack = new Stack<TreeNode>();
                while (stack.Count > 0 || head != null)
                {
                    if (head != null)
                    {
                        stack.Push(head);
                        head = head.Left;
                    }
                    else
                    {
                        head = stack.Pop();
                        Console.Write($"{head.Value}\t");
                        head = head.Right;
                    }
                }
            }

            Console.WriteLine("End");
        }

        // 遍历顺序：左右头
        public static void PostOrderUnRecursive1(TreeNode head)
        {
            if (head != null)
            {
                Stack<TreeNode> stack1 = new Stack<TreeNode>();
                Stack<TreeNode> stack2 = new Stack<TreeNode>();
                stack1.Push(head);
                while (stack1.Count > 0)
                {
                    head = stack1.Pop(); // 头 左 右
                    stack2.Push(head);

                    if (head.Left != null)
                    {
                        stack1.Push(head.Left);
                    }

                    if (head.Right != null)
                    {
                        stack1.Push(head.Right);
                    }
                }

                while (stack2.Count > 0)
                {
                    Console.Write($"{stack2.Pop().Value}\t");
                }
            }

            Console.WriteLine("End");
        }

        public static void PostOrderUnRecursive2(TreeNode head)
        {
            if (head != null)
            {
                Stack<TreeNode> stack = new Stack<TreeNode>();
                stack.Push(head);
                TreeNode curr = null;
                while (stack.Count > 0)
                {
                    curr = stack.Peek();
                    if (curr.Left != null
                        && head != curr.Left
                        && head != curr.Right)
                    {
                        stack.Push(curr.Left);
                    }
                    else if (curr.Right != null && head != curr.Right)
                    {
                        stack.Push(curr.Right);
                    }
                    else
                    {
                        Console.Write($"{stack.Pop().Value}\t");
                        head = curr;
                    }
                }
            }

            Console.WriteLine("End");
        }
    }
}