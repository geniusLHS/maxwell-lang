using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

Bootstrap(args[0]);

void Bootstrap(string filePath)
{
    try
    {
        if (File.Exists(filePath))
            Run(File.ReadAllText(filePath));
    }
    catch (IOException)
    {
        throw new Exception($"{filePath} : 잘못 지정된 파일 경로");
    }
};

void Run (string content) {
    var proceed = content.TrimEnd().Split("\n").Select(x => x.Trim()).ToArray();
    //Console.Write(proceed[^0]);
    //Console.Write(proceed.GetType()); //System.String[]
    int[] data = Enumerable.Repeat<int>(0,10000).ToArray<int>();

    data[0]=1;
    var gotoposition = new Dictionary<int, int>();
    var ptr1 = 0;
    var ptr2 = 1;
    var gotoptr = 0;
    var pointer = 0;
    

    while (pointer < proceed.Length)
    {

        var operation = proceed[pointer++];
        var evaludated = ParseOperation(operation);
        if (evaludated != null)
            Environment.Exit(evaludated.Value);
    }

    int? ParseOperation(string operation) 
    {
        if( operation.Contains("monopole") ) operation = operation.Split(new string[] { "monopole" }, StringSplitOptions.None).ToArray()[0];
        if( !( operation.Contains("=") )) return null;
        string[] operationDivideByEqual = operation.Split(new string[] { "=" }, StringSplitOptions.None).ToArray();
        if( operationDivideByEqual.Length != 2)
            Console.Write("Error (line " + pointer + ") : there is not exist '=' or more exist than one. \n");
        string command = operationDivideByEqual[0];
        string condition = operationDivideByEqual[1];

        var conditionptr = 0;
        bool isConditionTrue = true;
        bool isPlus = true;
        
        while( conditionptr < condition.Length )
        {
            if( condition[conditionptr] == '-' ) isPlus = !isPlus;
            if( conditionptr < condition.Length-4 && condition[conditionptr..(conditionptr+5)] == "∂E/∂t")
            {
                if( isPlus == true && data[ptr1] != data[ptr2] ) isConditionTrue = false;
                if( isPlus == false && data[ptr1] == data[ptr2] ) isConditionTrue = false;
                isPlus = true;
            }
            if( conditionptr < condition.Length-4 && condition[conditionptr..(conditionptr+5)] == "∂D/∂t")
            {
                if( isPlus == true && ptr1 != ptr2 ) isConditionTrue = false;
                if( isPlus == false && ptr1 == ptr2 ) isConditionTrue = false;
                isPlus = true;
            }
            if( conditionptr < condition.Length-4 && condition[conditionptr..(conditionptr+5)] == "∂B/∂t")
            {
                if( isPlus == true && data[ptr1] < 0 ) {
                    isConditionTrue = false;
                }
                if( isPlus == false && data[ptr1] >= 0 ) isConditionTrue = false;
                isPlus = true;
            }
            if( conditionptr < condition.Length-4 && condition[conditionptr..(conditionptr+5)] == "∂H/∂t")
            {
                if( isPlus == true && !gotoposition.ContainsKey(gotoptr) ) isConditionTrue = false;
                if( isPlus == false && gotoposition.ContainsKey(gotoptr) ) isConditionTrue = false;
                isPlus = true;
            }
            conditionptr++;
        }

        if(isConditionTrue == false) return null;
        //Console.Write(isConditionTrue);
        
        var commandptr = 0;
        isPlus = true;

        while( commandptr < command.Length )
        {
            if( command[commandptr] == '-' ) isPlus = !isPlus;

            //E definition
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇·E")
            {
                if( isPlus == true ) data[ptr1] = data[ptr1] + data[ptr2];
                else data[ptr1] = data[ptr1] - data[ptr2];
                isPlus = true;
            }
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇×E")
            {
                if( isPlus == true ) data[ptr1] = data[ptr1] * data[ptr2];
                else data[ptr1] = (int)(data[ptr1] / data[ptr2]);
                isPlus = true;
            }
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇²E")
            {
                if( isPlus == true ) 
                {
                    int t = data[ptr1];
                    data[ptr1] = data[ptr2];
                    data[ptr2] = t;
                }
                else Console.Write("Error (line " + pointer + ") : ∇²E can not be minus. \n");
            }

            // D definition
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇·D")
            {
                if( isPlus == true ) ptr1++;
                else ptr1--;
                isPlus = true;
            }
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇×D")
            {
                if( isPlus == true ) ptr2++;
                else ptr2--;
                isPlus = true;
            }
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇²D")
            {
                if( isPlus == true ) 
                {
                    int t = ptr1;
                    ptr1 = ptr2;
                    ptr2 = t;
                }
                else Console.Write("Error (line " + pointer + ") : ∇²D can not be minus. \n");
            }

            // B definition
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇·B")
            {
                if( isPlus == true ) Console.Write(data[ptr1]);
                else Console.Write("Error (line " + pointer + ") : ∇·B can not be minus. \n");
            }
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇×B")
            {
                if( isPlus == true ) Console.Write((char)(data[ptr1]));
                else Console.Write("Error (line " + pointer + ") : ∇×B can not be minus. \n");
            }
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇²B")
            {
                if( isPlus == true ) data[ptr1] = int.Parse(Console.ReadLine());
                else Console.Write("Error (line " + pointer + ") : ∇²B can not be minus. \n");
            }

            // H definition
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇·H")
            {
                if( isPlus == true ) gotoposition[gotoptr] = pointer;
                else Console.Write("Error (line " + pointer + ") : ∇·H can not be minus. \n");
            }
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇×H")
            {
                if( isPlus == true ) gotoptr++;
                else gotoptr--;
                isPlus = true;
            }
            if( commandptr < command.Length-2 && command[commandptr..(commandptr+3)] == "∇²H")
            {
                if( isPlus == true ) {pointer = gotoposition[gotoptr]; return null;}
                else Console.Write("Error (line " + pointer + ") : ∇²H can not be minus. \n");
            }
            commandptr++;
        }

        return null;
    }
}