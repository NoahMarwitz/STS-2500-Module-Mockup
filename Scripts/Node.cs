using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace STS_Web_App_Mockup.Scripts
{
    /*TODO: Set up like a linked list. 
     * Have a HEAD Node (Main Menu).
     * Have an END Node (Last slide).
     * Nodes need Load and Unload
     * Each Node Loads an associated XAML
     * 
     * Doc with Image
     * Doc
     * MC Question
     */
    class Node
    {
        public Node previous { get; set; }
        public Node next { get; set; }
        private string fileName;
        public string nodeName;
        public NodeType type;
        public int timer;

        private string TextBody = null;
        private string Image = null;
        private string answerA = null;
        private string answerB = null;
        private string answerC = null;
        private string answerD = null;
        private string correctAnswer = null;

        public Node(string data, Node prev, Node n, string name, NodeType t)
        {
            previous = prev;
            next = n;
            fileName = data;
            nodeName = name;
            type = t;
        }

        public void writeToData(string[] data)
        {
            switch(data[0])
            {
                case "Text":
                    type = NodeType.Text;
                    break;
                case "Image":
                    type = NodeType.Image;
                    break;
                case "Question":
                    type = NodeType.Question;
                    break;
                case "OnlyImage":
                    type = NodeType.OnlyImage;
                    break;
                default:
                    type = NodeType.Menu;
                    break;
            }
            TextBody = data[1];
            Image = data[2];
            answerA = data[3];
            answerB = data[4];
            answerC = data[5];
            answerD = data[6];
            correctAnswer = data[7].ToUpper();
            timer = Convert.ToInt32(data[8]);
            nodeName = data[9];
        }

        public override string ToString()
        {
            return "Previous Node: " + previous.nodeName + " | Next Node: " + next.nodeName + " | FILE NAME: " + fileName + " | CURRENT: " + nodeName;
        }

        public void UnloadContent()
        {
            Console.WriteLine("Unloaded Content for " + fileName);
        }

        public void LoadContent()
        {
            Console.WriteLine("Loaded in Content for " + fileName);
            Console.WriteLine("Node Type: " + type);
        }

        public string[] readData()
        {
            string[] returnval = new string[8];
            returnval[0] = TextBody;
            returnval[1] = Image;
            returnval[2] = answerA;
            returnval[3] = answerB;
            returnval[4] = answerC;
            returnval[5] = answerD;
            returnval[6] = correctAnswer;
            returnval[7] = timer.ToString();

            return returnval;
        }
    }

    public enum NodeType { Text, Image, OnlyImage, Question, Menu };
}
