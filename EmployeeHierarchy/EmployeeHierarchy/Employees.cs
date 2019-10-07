using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmployeeHierarchy
{
    public class Node
    {
        public static int salary_budget = 0;//intialize the value to zero. make it accessible anywhere
        public static bool is_salary_summation_applicable = false;//summation is only applicable whwn employee id is found
        public String employee_id;
        public String manager;
        public int salary;
        public Node[] next;
        public Node(String employee_id, String manager, int salary)
        {
            this.employee_id = employee_id;
            this.manager = manager;
            this.salary = salary;
            next = null;
        }
        public void print()
        {
            Console.Write("|" + employee_id + "," + manager + "," + salary + "||->");
            if (next != null)
            {
                for (int i = 0; i < next.Count(); i++)
                {
                    Console.Write("|level: " + i + "|");
                    next[i].print();
                }

            }
            else
            {
                Console.WriteLine();
            }
        }
        public int getSalaryBudget(string employee_id)
        {
            if (employee_id == this.employee_id)//check if the employee id matches
            {
                Node.is_salary_summation_applicable = true;//the node that has employee id is found so we can start summation
                //the above flag wil be used to stop transversing on root nodes after discovering an element
            }

            if (Node.is_salary_summation_applicable)//summation is applicable only when employee id is found
            {
                Node.salary_budget += salary;//increment the salary
                if (next != null)//keep going to find the subs
                {
                    for (int i = 0; i < next.Count(); i++)
                    {
                        next[i].getSalaryBudget(employee_id);//
                    }
                }
            }
            else
            {//keep looking
                if (next != null)//keep going to find the subs
                {
                    for (int i = 0; i < next.Count() && !is_salary_summation_applicable; i++)//this loop loops on the root or parent elements so it is crucial to stop it when element is found
                    {
                        next[i].getSalaryBudget(employee_id);
                    }

                }
            }
            return Node.salary_budget;
        }

        public void addEmployee(String employee_id, String manager, int salary)
        {
            if (next == null)
            {
                //if null then it is the end of list
                if (this.employee_id == manager)//check if employee is managed by the manager
                {
                    //if yes the employee should be added to this manager
                    next = new Node[] { new Node(employee_id, manager, salary) };
                    return;//since we have for loops it is a wise idea to return to terminate them after adding element
                }
                else
                {

                }
            }
            else
            {
                if (this.employee_id == manager)//check if employee is managed by the manager
                {
                    //if yes the employee should be added to this manager
                    //in this case there is already an employee been managed by this manager
                    Node[] existingArray = next;
                    Node[] element_to_add = new Node[] { new Node(employee_id, manager, salary) };
                    Node[] merged_array = new Node[existingArray.Length + element_to_add.Length];
                    Array.Copy(existingArray, merged_array, existingArray.Length);
                    Array.Copy(element_to_add, 0, merged_array, existingArray.Length, element_to_add.Length);
                    next = merged_array;//overite the existing one
                    return;//since we have for loops it is a wise idea to return to terminate them after adding element

                }
                else
                {
                    //it means the employee may be managed by child tree
                    //so keep moving forward to search
                    for (int i = 0; i < next.Count(); i++)//loop through nodes in next
                    {
                        next[i].addEmployee(employee_id, manager, salary);
                    }
                }



            }
        }
    }

    class TopologicalSorter
    {


        private readonly int[] vertices; //list of vertices
        private readonly int[,] matrix; // adjacency matrix
        private int number_of_verteces; //tracks the number of current verteces
        private readonly int[] sorted_array;//list of the sorted elements



        public TopologicalSorter(int size)
        {
            vertices = new int[size];
            matrix = new int[size, size];//create a matrix of n by n where n is size of fields to be passed
            number_of_verteces = 0;//intialize the number of vertices to at the begining. Note that there are zero vertices at begining
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = 0;//set all values to zero in the matrix
            sorted_array = new int[size]; //create a list with size of elements to be added
        }



        public int AddVertex(int vertex)
        {
            vertices[number_of_verteces++] = vertex;
            return number_of_verteces - 1;
        }

        public void AddEdge(int start, int end)
        {
            matrix[start, end] = 1;
        }

        public int[] Sort() // sort elements using toplogical sort
        {
            while (number_of_verteces > 0) // while vertices remain,
            {
                int currentVertex = noSuccessors();//look for vertex with no successor or -1 in case of a cycle
                if (currentVertex == -1) // trow an exception since it is a cycle                
                    throw new Exception("Graph has circular references or cycles");

                // insert vertex label in sorted array (start at end)
                sorted_array[number_of_verteces - 1] = vertices[currentVertex];

                deleteVertex(currentVertex); // delete vertex since it has been added to the sorted array
            }

            //after all vertecies have been added to the sorted array and deleted it is time to return the sorted list
            return sorted_array;//return the sorted list
        }




        private int noSuccessors()//the method looks for vertices with no successor and returns -1 if vert is not found
        {
            for (int row = 0; row < number_of_verteces; row++)//loop through all number of vertices
            {
                bool isEdge = false; //set the flag to false since we are going to check if the vertex has an edge. edge from row to column in adjMat
                for (int col = 0; col < number_of_verteces; col++)
                {
                    if (matrix[row, col] > 0) //if greater than zero then it is an edge to another, meaning it has an edge
                    {
                        isEdge = true;//set te flag to true since this vertex has an edge
                        break; //since the vertex has an edge then break to save CPU cycles and keep looking for vertex without successor or edge
                    }
                }
                if (!isEdge) // if the vertex has no edge then it has no successor so return the row
                    return row;//return the vertex that has no successor. Return statement also short circuits the system if loop is not complete to prevent wastage of CPU cycles
            }
            return -1; //search for no successor vertex was complete but no one was found
        }

        private void deleteVertex(int delVert)
        {
            // if it is not the last vertex then you can delete it
            if (delVert != number_of_verteces - 1)
            {
                for (int j = delVert; j < number_of_verteces - 1; j++)
                    vertices[j] = vertices[j + 1];

                for (int row = delVert; row < number_of_verteces - 1; row++)
                    moveRowUp(row, number_of_verteces);

                for (int col = delVert; col < number_of_verteces - 1; col++)
                    moveColLeft(col, number_of_verteces - 1);
            }
            number_of_verteces--; // subtract one vertex since it has been added to sorted area and deleted if neccessary
        }

        private void moveRowUp(int row, int length)
        {
            for (int col = 0; col < length; col++)
                matrix[row, col] = matrix[row + 1, col];//shifts the rows below upwards
        }

        private void moveColLeft(int col, int length)
        {
            for (int row = 0; row < length; row++)
                matrix[row, col] = matrix[row, col + 1];//shifts remaining columns leftwards to compesate for deleted vertex
        }


    }



    public class Employees
    {
        Node node;
        public Employees(String cvs)
        {
            /*check if string is empty or null*/
            if (cvs == null || cvs == "")
            {
                throw new ArgumentException("cvs string argument", "cvs string argument cannot be null");
            }
            //this.cvs = cvs;
            /*break line by line*/
            //string line;
            List<String> lines = new List<String>();//List for storing all lines


            StringReader sr = new StringReader(cvs);
            String line;
            while ((line = sr.ReadLine()) != null)//read line by line
            {

                int commacount = line.Split(',').Length - 1;//get number of commas
                if (commacount != 2)//if comma count is not equal to two then it is not valid line
                {
                    throw new ArgumentException("cvs string argument", "The line formart not supported. Line-> " + line);
                }
                //at this point we know the line has the format we need so we check if salary has valid int
                String[] spearator = { "," };
                String[] strlist = line.Split(spearator, StringSplitOptions.None);//we also need empty entries in this case
                String salary = strlist[2];
                int n;
                bool isInt = int.TryParse(salary, out n);//check validity of integer by trying to parse it
                if (!isInt)//if not integer throw exception otherwise continue
                {
                    throw new ArgumentException("cvs string argument", "The salary value is not valid. Line-> " + line);
                }
                String employee = strlist[0];//avoid assigning the values in the loop to avoid wastege of CPU cycles
                String manager = strlist[1];//avoid assigning the values in the loop to avoid wastege of CPU cycles

                for (int i = 0; i < lines.Count; i++)//check if there is duplicate line and if there is multiple managers managing one emloyee
                {

                    if (line == lines[i])//if line exist then this is a duplicate so throw exception
                    {
                        throw new ArgumentException("cvs string argument", "Duplicate lines are not allowed. Line-> " + line);
                    }

                    /*check if there is multiple managers managing one emloyee
                     * This can be archieved by making sure only one line defining employee occurs.
                     * This also eliminates possibility of one employee been paid twice
                     */
                    String[] existinglist = lines[i].Split(spearator, StringSplitOptions.None);//we also need empty entries in this case
                    if (employee == existinglist[0])
                    {
                        throw new ArgumentException("cvs string argument", "One employee cannot be defined twise in Line-> " +
                            line + " and Line-> " + lines[i] + ". This can lead to one employee been managed more than onces or having different salaries");
                    }
                    //Make sure there is no more than one manager
                    if (manager == "" && manager == existinglist[1])// check first if it is a CEO before procedding to check if already. Short circuting will help to reduce CPU since most of employees are not CEO
                    {
                        throw new ArgumentException("cvs string argument", "There cannot be more than one CEO. Line-> " +
                            line + " and Line-> " + lines[i]);
                    }

                }
                lines.Add(line);//go ahead and save the line
            }
            Boolean is_ceo_existing = false;
            Boolean is_manager_existing_as_employee = false;

            for (int i = 0; i < lines.Count; i++)//loop through to make sure there is atleast one CEO and no manager who is not employees
            {


                String[] spearator = { "," };
                String[] existinglist = lines[i].Split(spearator, StringSplitOptions.None);//we also need empty entries in this case
                String manager_to_check = existinglist[1];
                String employee_to_check = existinglist[0];

                if (manager_to_check == "")//check if it is CEO
                {
                    is_ceo_existing = true;//CE0 is found

                }
                else
                {
                    //Console.WriteLine("REached");
                    for (int j = 0; j < lines.Count; j++)
                    {
                        String[] strlist = lines[j].Split(spearator, StringSplitOptions.None);//we also need empty entries in this
                        if (manager_to_check == strlist[0])//check if manager matches employee
                        {
                            //Console.WriteLine("pass " + manager_to_check + "  " + strlist[0]);
                            is_manager_existing_as_employee = true;
                            //break; //stop the inner loop because we got what we are looking for. This prevents wastage of CPU cycle which van slow the program in large lists
                            if (employee_to_check == strlist[1])//no circular reference between two employees
                            {
                                throw new ArgumentException("cvs string argument", "Circular reference found in line-> " + lines[i] + "and line-> " + lines[j]);
                            }

                        }


                        //Console.WriteLine( manager_to_check + "  " + strlist[0]);

                    }
                    if (!is_manager_existing_as_employee)//check if Manager exists as employee
                    {

                        throw new ArgumentException("cvs string argument", "The manager does not exist as employee ");
                    }
                    is_manager_existing_as_employee = false;//reset again

                }

            }

            if (!is_ceo_existing)
            {
                throw new ArgumentException("cvs string argument", "There is no CEO in the list ");
            }




            List<Field> fields = new List<Field>();

            for (int i = 0; i < lines.Count; i++)
            {
                String[] spearator = { "," };
                String[] existinglist = lines[i].Split(spearator, StringSplitOptions.None);//we also need empty entries in this case
                String manager_to_add = existinglist[1];
                String employee_to_add = existinglist[0];
                int salry_to_add = Int32.Parse(existinglist[2]);

                //this.node.addEmployee(employee_to_add, manager_to_add, salry_to_add);
                //Console.WriteLine(employee_to_add+","+manager_to_add+","+salry_to_add);

                if (manager_to_add == "")
                {// CEO is the only person who doesnt depend on manager

                    fields.Add(new Field() { Name = lines[i] });
                }
                else
                {
                    for (int j = 0; j < lines.Count; j++)
                    {
                        String[] strlist = lines[j].Split(spearator, StringSplitOptions.None);//we also need empty entries in this case

                        String employee = strlist[0];
                        if (manager_to_add == employee)
                        {
                            fields.Add(new Field() { Name = lines[i], DependsOn = new[] { lines[j] } });
                            break;//stop the inner loop because we got what we are looking for. This prevents wastage of CPU cycle which van slow the program in large lists
                        }
                    }
                }


            }

            //sort out the lines depending on managers
            int[] sortOrder = getTopologicalSortOrder(fields);//sort the fields
            for (int i = sortOrder.Length - 1; i >= 0; i--)
            {
                var field = fields[sortOrder[i]];
                String[] spearator = { "," };
                String[] existinglist = field.Name.Split(spearator, StringSplitOptions.None);//we also need empty entries in this case
                String manager_id = existinglist[1];
                String employee_id = existinglist[0];
                int salary = Int32.Parse(existinglist[2]);
                //first find CEO, add him/her to the node tree and then remove him/her from the list
                if (i == sortOrder.Length - 1)//the last field on the list is for CEO line
                {
                    this.node = new Node(employee_id, manager_id, salary);//CEO i added to the list
                }
                else//add other employeees
                {
                    this.node.addEmployee(employee_id, manager_id, salary);
                }
                /*if (field.DependsOn != null)//fortesting purposes only
                {
                    foreach (var item in field.DependsOn)
                    {
                        Console.WriteLine(" -{0}", item);
                    }
                }*/
            }
            //this.node.print();//for testing purposes only

        }

        public int getSalaryBudget(String employee_id)
        {
            Node.is_salary_summation_applicable = false; //reset this flag especialy when using method more than once.
            //in the second time use the flag will be true making the samystem to think the element has already been found
            int salary_budget = this.node.getSalaryBudget(employee_id);//hold salary temporarly before clearing it
            Node.salary_budget = 0;//reset the salary before anothe querry
            return salary_budget;
        }






        private static int[] getTopologicalSortOrder(List<Field> fields)
        {
            TopologicalSorter graph = new TopologicalSorter(fields.Count);
            Dictionary<string, int> indexes = new Dictionary<string, int>();

            //loop through to add vertices
            for (int i = 0; i < fields.Count; i++)
            {
                indexes[fields[i].Name.ToLower()] = graph.AddVertex(i);
            }

            //add edges
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i].DependsOn != null)//add an edge only when there is dependence
                {
                    for (int j = 0; j < fields[i].DependsOn.Length; j++)//add edges to all dependencies 
                    {
                        graph.AddEdge(i,
                            indexes[fields[i].DependsOn[j].ToLower()]);
                    }
                }
            }

            int[] result = graph.Sort();
            return result;

        }


        class Field
        {
            public string Name { get; set; }
            public string[] DependsOn { get; set; }
        }



    }

}
