using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LB5_LOBKOVA
{
    public partial class UserControl1 : UserControl
    {
        private DispatcherTimer _timer;
        private List<Horse> _horses;
        private const double CircleRadius = 200; // Радиус круга

        // Динамический расчет центра для Canvas
        private double CenterX => (HorseCanvas.ActualWidth - CircleRadius * 2) / 2;
        private double CenterY => (HorseCanvas.ActualHeight - CircleRadius * 2) / 2;

        public UserControl1()
        {
            InitializeComponent();
            InitializeHorses();
            SizeChanged += (s, e) => InitializeHorses(); // Пересчитывать при изменении размера
        }

        private void InitializeHorses()
        {
            HorseCanvas.Children.Clear();
            _horses = new List<Horse>();
            Random random = new();

            // Добавляем круг на холст
            Ellipse raceTrack = new()
            {
                Width = CircleRadius * 2,
                Height = CircleRadius * 2,
                Stroke = System.Windows.Media.Brushes.Purple,
                StrokeThickness = 4,
                Fill = System.Windows.Media.Brushes.Lavender
            };

            Canvas.SetLeft(raceTrack, CenterX);
            Canvas.SetTop(raceTrack, CenterY);
            HorseCanvas.Children.Add(raceTrack);

            for (int i = 0; i < 5; i++)
            {
                // Создаём объект лошади
                Horse horse = new Horse
                {
                    X = CenterX + CircleRadius + CircleRadius * Math.Cos(i * 2 * Math.PI / 5),
                    Y = CenterY + CircleRadius + CircleRadius * Math.Sin(i * 2 * Math.PI / 5),
                    Speed = random.Next(1, 5) / 10.0 // Скорость в диапазоне 0.1–0.5
                };
                _horses.Add(horse);

                // Визуализация лошади
                Image horseImage = new Image
                {
                    Width = 40,
                    Height = 40,
                    Source = new BitmapImage(new Uri("https://kartinki.pics/uploads/posts/2022-12/1670311048_1-kartinkin-net-p-loshadka-kartinka-dlya-detei-pinterest-1.jpg"))
                };

                // Привязка данных
                horseImage.DataContext = horse;
                horseImage.SetBinding(Canvas.LeftProperty, new System.Windows.Data.Binding("X"));
                horseImage.SetBinding(Canvas.TopProperty, new System.Windows.Data.Binding("Y"));

                // События мыши
                horseImage.MouseLeftButtonDown += (s, e) =>
                {
                    MessageBox.Show($"Speed: {horse.Speed:F2}"); // Вывод скорости лошади
                };

                horseImage.MouseRightButtonDown += (s, e) =>
                {
                    MessageBox.Show($"Position: ({horse.X:F2}, {horse.Y:F2})"); // Вывод позиции лошади
                };

                HorseCanvas.Children.Add(horseImage);
            }
        }

        public void StartRace()
        {
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            _timer.Tick += (s, e) =>
            {
                foreach (var horse in _horses)
                {
                    horse.Move(0.1, CenterX + CircleRadius, CenterY + CircleRadius, CircleRadius);
                }
            };
            _timer.Start();
        }

        public void PauseRace()
        {
            _timer?.Stop();
        }

        public void ResetRace()
        {
            _timer?.Stop();
            InitializeHorses();
        }
    }

    public class Horse : DependencyObject
    {
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Horse), new PropertyMetadata(0.0));

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Horse), new PropertyMetadata(0.0));

        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(double), typeof(Horse), new PropertyMetadata(0.0));

        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public double Speed
        {
            get => (double)GetValue(SpeedProperty);
            set => SetValue(SpeedProperty, value);
        }

        public void Move(double angleIncrement, double centerX, double centerY, double radius)
        {
            double angle = Math.Atan2(Y - centerY, X - centerX) + angleIncrement * Speed;
            X = centerX + radius * Math.Cos(angle);
            Y = centerY + radius * Math.Sin(angle);
        }
    }
}
