using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace STS_Web_App_Mockup.Scripts
{
    class NodeManager
    {
        private List<Node> nodes;
        public Node head;
        public Node tail;
        private Node curNode;

        public NodeManager()
        {
            nodes = new List<Node>();
            curNode = null;
        }

        public void AddNode(Node n)
        {
            //Assemble connections
            if (nodes.Count > 0)
            {
                n.previous = nodes.Last();
                n.next = null;
                nodes.Last().next = n;
                nodes.Add(n);
                tail = n;
            }
            else
            {
                n.previous = null;
                n.next = null;
                nodes.Add(n);
                head = n;
            }
        }

        public Node slideN(int index)
        {
            return nodes[index];
        }

        public void transition(bool right)
        {
            if (right)
            {
                if (curNode == null)
                {
                    curNode = head;
                    head.LoadContent();
                }
                else
                {
                    if (curNode.next != null)
                    {
                        curNode.UnloadContent();
                        curNode.next.LoadContent();
                        curNode = curNode.next;
                    }
                    else
                    {
                        Console.WriteLine("Hit Tail");
                    }
                }
            }
            else
            {
                if (curNode == null)
                {
                    curNode = tail;
                    head.LoadContent();
                }
                else
                {
                    if (curNode.previous != null)
                    {
                        curNode.UnloadContent();
                        curNode.previous.LoadContent();
                        curNode = curNode.previous;
                    }
                    else
                    {
                        Console.WriteLine("Hit Head");
                    }
                }
            }
        }

        public void goToHead()
        {
            if(curNode != null)
                curNode.UnloadContent();
            head.LoadContent();
            curNode = head;
        }

        public bool goToN(string slideName)
        {
            Node checkNode = head;
            bool found = false;
            while (checkNode.next != null && !found)
            {
                checkNode = checkNode.next;
                if (checkNode.nodeName == slideName)
                    found = true;
            }

            if (found)
            {
                curNode.UnloadContent();
                checkNode.LoadContent();
                curNode = checkNode;
            }

            return found;
        }

        public string[] generateNodeData(int index)
        {
            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory()+"\\Slides\\Slide"+index+".txt");
            int repeat = 0;

            string[] returnval = new string[10];
            try
            {
                do
                {
                    string curLine = reader.ReadLine();
                    string[] items = curLine.Split(new char[] { ' ' }, 2);
                    Console.WriteLine("READING " + curLine);

                    switch (items[0])
                    {
                        case "Type":
                            returnval[0] = items[1];
                            break;
                        case "Text":
                            repeat = Convert.ToInt32(items[1]);
                            string retString = "";
                            while(repeat > 0)
                            {
                                repeat--;
                                if(reader.Peek() != -1)
                                {
                                    curLine = reader.ReadLine();
                                    retString += curLine + "\n";
                                }
                            }
                            returnval[1] = retString;
                            break;
                        case "Image":
                            returnval[2] = items[1];
                            break;
                        case "AnswerA":
                            returnval[3] = items[1];
                            break;
                        case "AnswerB":
                            returnval[4] = items[1];
                            break;
                        case "AnswerC":
                            returnval[5] = items[1];
                            break;
                        case "AnswerD":
                            returnval[6] = items[1];
                            break;
                        case "Correct":
                            returnval[7] = items[1];
                            break;
                        case "Timer":
                            returnval[8] = items[1];
                            break;
                        case "Title":
                            returnval[9] = items[1];
                            break;

                        default:
                            Console.WriteLine("What is this?"); //OWO
                            break;
                    }
                }
                while (reader.Peek() != -1);
            }
            catch
            {
                Console.WriteLine("Turns out no file was found");
            }
            finally
            {
                reader.Close();
            }
            return returnval;
        }


        public Node currentNode()
        {
            Node n = null;
            n = curNode;


            return n;
        }
    }
}
