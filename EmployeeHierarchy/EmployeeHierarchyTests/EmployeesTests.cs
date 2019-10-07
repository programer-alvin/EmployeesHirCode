using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EmployeeHierarchy.Tests
{
    [TestClass()]
    public class EmployeesTests
    {
        [TestMethod()]
        public void EmployeesOkayTest()//test data is valid
        {
            String cvs = "Employee4,Employee2,500\n" +
                "Employee3,Employee1,800\n" +
                "Employee1,,1000\n" +
                "Employee5,Employee1,500\n" +
                "Employee2,Employee1,500";
            try
            {
                Employees employees = new Employees(cvs);
            }
            catch (Exception ex)
            {
                Assert.Fail("No exception is expected" + ex.ToString());
            }
        }

        [TestMethod()]
        public void UnorderedEmployeesOkayTest()//test data is valid
        {
            String cvs = "Employee4,Employee2,500\n" +
              "Employee3,Employee1,800\n" +
              "Employee1,,1000\n" +
              "Employee5,Employee1,500\n" +
              "Employee2,Employee1,500\n";
            try
            {
                Employees employees = new Employees(cvs);
            }
            catch (Exception ex)
            {
                Assert.Fail("No exception is expected" + ex.ToString());
            }
        }

        [TestMethod()]
        public void EmployeesSalaryTest()//test data is valid
        {
            String cvs = "Employee1,,1000\n" +
                "Employee2,Employee1,500\n" +
                "Employee5,Employee1,500\n" +
                "Employee3,Employee1,800\n" +
                "Employee4,Employee2,500";
            try
            {
                Employees employees = new Employees(cvs);
                Assert.AreEqual(3300, employees.getSalaryBudget("Employee1"));
                Assert.AreEqual(1000, employees.getSalaryBudget("Employee2"));
                Assert.AreEqual(800, employees.getSalaryBudget("Employee3"));
                Assert.AreEqual(500, employees.getSalaryBudget("Employee4"));
                Assert.AreEqual(500, employees.getSalaryBudget("Employee5"));
            }
            catch (Exception ex)
            {
                Assert.Fail("No exception is expected" + ex.ToString());
            }
        }

        [TestMethod()]
        public void UnorderEmployeesListSalaryTest()//test data is valid
        {
            String cvs = "Employee4,Employee2,500\n" +//test data extracted from question 2 part b of SDE 
              "Employee3,Employee1,500\n" +
              "Employee6,Employee2,500\n" +
              "Employee1,,1000\n" +
              "Employee5,Employee1,500\n" +
              "Employee2,Employee1,800\n";
            try
            {
                Employees employees = new Employees(cvs);
                Assert.AreEqual(3800, employees.getSalaryBudget("Employee1"));//employee1 tested
                Assert.AreEqual(1800, employees.getSalaryBudget("Employee2"));//employee2 tested
                Assert.AreEqual(500, employees.getSalaryBudget("Employee3"));//employee3 tested
                Assert.AreEqual(500, employees.getSalaryBudget("Employee4"));
                Assert.AreEqual(500, employees.getSalaryBudget("Employee5"));
                Assert.AreEqual(500, employees.getSalaryBudget("Employee6"));
            }
            catch (Exception ex)
            {
                Assert.Fail("No exception is expected" + ex.ToString());
            }
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Null is not allowed")]
        public void NullCVSStringConstructor()
        {
            String cvs = null;
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Epmty is not allowed")]
        public void EmptyCVSStringConstructor()
        {
            String cvs = "";
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid Salary is not allowed")]
        public void InvalidSalaryStringConstructor()
        {
            String cvs = "Employee1,,10h0";
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Multiple managers to one employee is not allowed")]
        public void ManagedByMoreManagersConstructor()
        {
            String cvs = "Employee1,,1000\n" +
                "Employee2,Employee1,500\n" +
                "Employee4, Employee2,500\n" +
                "Employee4, Employee1,500";
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "more than one ceo is not accepted")]
        public void MultipleCEOConstructor()
        {
            String cvs = "Employee1,,1000\n" +
                "Employee2,,1000";
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "circular reference is not accepted")]
        public void CircularReferenceConstructor()
        {
            String cvs = "Employee1,,1000\n" +
                "Employee1,Employee2,500\n" +
                "Employee2,Employee1,500";
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Graph has circular references or cycles")]
        public void CircularReferenceForMoreThanTwoEmployeeSort()
        {
            String cvs = "Employee1,,1000\n" +
                "Employee2,Employee5,500\n" +
                "Employee3,Employee2,500\n" +
                "Employee5,Employee4,500\n" +
                "Employee4,Employee3,500";
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "no manager who is not employee")]
        public void InvalidManagerConstructor()
        {
            String cvs = "Employee1,,1000\n" +
                "Employee2,Employee5,500\n" +
                "Employee3,Employee2,500\n" +
                "Employee4,Employee3,500";
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "unacceptable columns")]
        public void InvalidLineConstructor()
        {
            String cvs = "Employee1,,1,000";
            Employees employees = new Employees(cvs);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "duplicate employees")]
        public void DuplicateEmployeeLineConstructor()
        {
            String cvs = "Employee4, Employee2,500\n" +
                "Employee3, Employee1, 800\n" +
                "Employee1,,1000\n" +
                "Employee5,Employee1,500\n" +
                "Employee3,Employee2,500\n" +
                "Employee2,Employee1,500";
            Employees employees = new Employees(cvs);
        }

    }
}