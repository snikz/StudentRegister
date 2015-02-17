using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Diagnostics;
namespace Assignment_7_db
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext dx;
        public MainWindow()
        {
            InitializeComponent();
            dx = new DataClasses1DataContext();
            Table<student> studenter = dx.students;
            gridStudent.DataContext = studenter;
            gridCourse.DataContext = dx.courses;
            gridCourse.Visibility = Visibility.Hidden;
            gridGrade.Visibility = Visibility.Hidden;
            gridStudentGrade.Visibility = Visibility.Hidden;
            gridStudentGradeCourse.Visibility = Visibility.Hidden;
            gridStudentFailed.Visibility = Visibility.Hidden;
        }

        private void Student_Click(object sender, RoutedEventArgs e)
        {
            gridStudent.Visibility = Visibility.Visible;
            gridGrade.Visibility = Visibility.Hidden;
            gridCourse.Visibility = Visibility.Hidden;
            gridStudentGrade.Visibility = Visibility.Hidden;
            gridStudentGradeCourse.Visibility = Visibility.Hidden;
            gridStudentFailed.Visibility = Visibility.Hidden;
            gridStudent.DataContext = dx.students;
        }

        private void Grade_Click(object sender, RoutedEventArgs e)
        {
            gridStudent.Visibility = Visibility.Hidden;
            gridGrade.Visibility = Visibility.Visible;
            gridCourse.Visibility = Visibility.Hidden;
            gridStudentGrade.Visibility = Visibility.Hidden;
            gridStudentGradeCourse.Visibility = Visibility.Hidden;
            gridStudentFailed.Visibility = Visibility.Hidden;
            gridGrade.DataContext = dx.grades;
        }

        private void Course_Click(object sender, RoutedEventArgs e)
        {
            gridStudent.Visibility = Visibility.Hidden;
            gridGrade.Visibility = Visibility.Hidden;
            gridCourse.Visibility = Visibility.Visible;
            gridStudentGrade.Visibility = Visibility.Hidden;
            gridStudentGradeCourse.Visibility = Visibility.Hidden;
            gridStudentFailed.Visibility = Visibility.Hidden;
            gridCourse.DataContext = dx.courses;
        }

        private void SearchName_Click(object sender, RoutedEventArgs e)
        {
            gridStudent.Visibility = Visibility.Visible;
            gridGrade.Visibility = Visibility.Hidden;
            gridCourse.Visibility = Visibility.Hidden;
            gridStudentGrade.Visibility = Visibility.Hidden;
            gridStudentGradeCourse.Visibility = Visibility.Hidden;
            gridStudentFailed.Visibility = Visibility.Hidden;
            gridStudent.DataContext = dx.students.Where(stud => stud.studentname.Contains(searchName.Text));
        }

        private void gridGrade_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            grade temp = (grade)gridGrade.SelectedItem;
            var test = dx.students.Select(stud => new { stud.studentname, stud.id })
                .Join(dx.grades, stud => stud.id, gr => gr.studentid, (stud, gr) => new { stud.studentname, gr.grade1, gr.coursecode })
                .Where(gr => gr.grade1.CompareTo(temp.grade1) <= 0)
                .Join(dx.courses, gr => gr.coursecode, course => course.coursecode, (gr, course) => new { gr.studentname, gr.grade1, course.coursename });

            gridStudentGradeCourse.DataContext = test;

            gridGrade.Visibility = Visibility.Hidden;
            gridStudentGradeCourse.Visibility = Visibility.Visible;
            
        }

        private void gridCourse_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            gridGrade.Visibility = Visibility.Hidden;
            gridStudentGrade.Visibility = Visibility.Visible;
            course temp = (course)gridCourse.SelectedItem;

            //2 MÅTER Å GJØRE DET PÅ!! LÆR BEGGE!!

            //var test = from stud in dx.students
            //           join gr in dx.grades
            //           on stud.id equals gr.studentid
            //           join c in dx.courses
            //           on gr.coursecode equals c.coursecode
            //           select new { stud.studentname, gr.grade1, c.coursename};

            //gridStudentGrade.DataContext = test;
            var test = dx.students.Select(stud => new { stud.studentname, stud.id })
                .Join(dx.grades, stud => stud.id, gr => gr.studentid, (stud, gr)
                => new { stud.studentname, gr.grade1, gr.coursecode })
                .Join(dx.courses, gr => gr.coursecode, course => course.coursecode, (gr, course)
                => new { gr.studentname, gr.grade1, course.coursename });
            gridStudentGrade.DataContext = test;
        }

        private void Failed_Click(object sender, RoutedEventArgs e)
        {
            gridStudent.Visibility = Visibility.Hidden;
            gridCourse.Visibility = Visibility.Hidden;
            gridGrade.Visibility = Visibility.Hidden;
            gridStudentGrade.Visibility = Visibility.Hidden;
            gridStudentGradeCourse.Visibility = Visibility.Hidden;
            gridStudentFailed.Visibility = Visibility.Visible;

            var test = dx.students.Select(stud => new { stud.studentname, stud.id })
                .Join(dx.grades, stud => stud.id, gr => gr.studentid, (stud, gr) => new { stud.studentname, gr.coursecode, gr.grade1 })
                .Where(gr => gr.grade1.CompareTo('f') == 0)
                .Join(dx.courses, gr => gr.coursecode, course => course.coursecode, (gr, course) => new { gr.studentname, gr.grade1, course.coursename });
            gridStudentFailed.DataContext = test;
        }

        private void Student_Click(object sender, MouseButtonEventArgs e)
        {
            student temp = (student)gridStudent.SelectedItem;
            var test = dx.students.Select(stud => new { stud.id, stud.studentname })
                .Where(stud => stud.studentname == temp.studentname)
                .Join(dx.grades, stud => stud.id, gr => gr.studentid, (stud, gr) => new { stud.studentname, gr.grade1, gr.coursecode })
                .Join(dx.courses, gr => gr.coursecode, course => course.coursecode, (gr, course) => new { gr.studentname, gr.grade1, course.coursename });
            gridStudentGradeCourse.DataContext = test;
            gridStudentGradeCourse.Visibility = Visibility.Visible;
            gridStudent.Visibility = Visibility.Hidden;
        }

        private void searchName_GotFocus(object sender, RoutedEventArgs e)
        {
            searchName.Text = "";
        }

        private void searchName_LostFocus(object sender, RoutedEventArgs e)
        {
            searchName.Text = "Write name";
        }
    }
}
