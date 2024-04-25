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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JUSTDOIT
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private bool panel_stat = false;
        private int global_id = 1;


        public MainMenu()
        {
            InitializeComponent();


            var data = manager.load_data();
            if (data != null )
            {
                foreach (Task task in data.Items)
                {
                    TaskList.Items.Add(task);
                }
                try
                {
                    global_id = (data.Items[data.Items.Count - 1] as Task).Id + 1;
                }
                catch { }
            }

            var stat = manager.load_stat();
            if (stat != null)
            {
                TotalTask.Text = stat.Split(':')[0];
                DoesTask_.Text = stat.Split(':')[1];
                DropTask.Text = stat.Split(':')[2];
            }


            manager.TaskList = TaskList;
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            Task task1 = create_task(global_id, "_", "_");
            TaskList.Items.Add(task1);
            global_id++;
        }


        private Task create_task(int id, string name, string disc)
        {
            Task task1 = new Task();

            task1.Id = id;

            task1.Name = name;

            task1.Discription = disc;

            task1.Time = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

            Canvas can = new Canvas();
            can.Name = "Can" + id.ToString();
            task1.canvas = can;

            return task1;
        }



        private void DoesTask(object sender, RoutedEventArgs e)
        {

            var button = sender as CheckBox;
            var code = (Task)button.DataContext;
            //MessageBox.Show(code.Id.ToString() + code.Doing.ToString());


            var crest = code.crest;
            crest.Points = new PointCollection() { new Point(190, 12), new Point(190, 145), new Point(-165, 12), new Point(-165, 145) };
            crest.Stroke = Brushes.Purple;
            crest.StrokeThickness = 2;

            // Устанавливаем позицию крестика на Canvas
            Canvas.SetLeft(crest, code.canvas.ActualWidth / 2 - 10); // центрируем по горизонтали
            Canvas.SetTop(crest, code.canvas.ActualHeight / 2 - 30); // центрируем по вертикали

            if (!code.Doing)
            {
                code.canvas.Children.Add(crest);
            }
            else
            {
                code.canvas.Children.Remove(crest);
            }




            code.Doing = !code.Doing;

        }

        private void Polygon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int n;
            if (!panel_stat)
            {
                n = 200;
                panel_stat = true;
            }
            else
            {
                n = -200;
                panel_stat = false;
            }

            //myRectangle.Margin = new Thickness(n, 0, 0, 0);
            //PolMovePanel.Margin = new Thickness(n, 0, 0, 0);

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = Canvas.GetLeft(PanelInfo); // текущее положение элемента
            animation.To = Canvas.GetLeft(PanelInfo) + n; // новое положение элемента (на 100 пикселей влево)
            animation.Duration = TimeSpan.FromSeconds(1); // длительность анимации (1 секунда)

            DoubleAnimation animation2 = new DoubleAnimation();
            animation2.From = Canvas.GetLeft(PolMovePanel); // текущее положение элемента
            animation2.To = Canvas.GetLeft(PolMovePanel) + n; // новое положение элемента (на 100 пикселей влево)
            animation2.Duration = TimeSpan.FromSeconds(1); // длительность анимации (1 секунда)

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Children.Add(animation2);


            Storyboard.SetTarget(animation, PanelInfo);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));


            Storyboard.SetTarget(animation2, PolMovePanel);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(Canvas.LeftProperty));


            storyboard.Begin();
        }

        private void Del_task_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var code = (Task)button.DataContext;

            for (int i = TaskList.Items.IndexOf((Task)button.DataContext); i < TaskList.Items.Count; i++)
            {
                (TaskList.Items[i] as Task).Id -= 1;
            }

            TaskList.Items.Remove((Task)button.DataContext);

            CollectionViewSource.GetDefaultView(TaskList.Items).Refresh();

            TotalTask.Text = (Convert.ToInt32(TotalTask.Text) + 1).ToString();

            if(code.Doing == true)
            {
                DoesTask_.Text = (Convert.ToInt32(DoesTask_.Text) + 1).ToString();
            }
            else
            {
                DropTask.Text = (Convert.ToInt32(DropTask.Text) + 1).ToString();
            }

            global_id--;
        }

        public string give_stat()
        {
            return $"{TotalTask.Text}:{DoesTask_.Text}:{DropTask.Text}";
        }
    }
}
