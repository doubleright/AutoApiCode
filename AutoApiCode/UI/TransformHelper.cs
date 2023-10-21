using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AutoApiCode.UI
{
    public class TransformHelper
    {
        public class Story
        {
            FrameworkElement _control;
            Storyboard _storyboard;

            public Story(FrameworkElement control, Storyboard storyboard)
            {
                this._control = control;
                this._storyboard = storyboard;
            }

            public void BeginStoryboard()
            {
                _storyboard.Begin(_control, true);// true 表示动画可控
            }

            public void EndStoryboard()
            {
                _storyboard.Stop(_control);
            }
        }

        
        public static Story GetRotateStory(FrameworkElement control)
        {
            RotateTransform rotate = new RotateTransform();   //做旋转动
            DoubleAnimation da = new DoubleAnimation();   //数值类型
            Storyboard story = new Storyboard(); //故事板

            control.RenderTransformOrigin = new Point(0.5, 0.5);
            //rt_FanRotate.CenterX = 0;     //旋转中心
            //rt_FanRotate.CenterY = 0;
            control.RenderTransform = rotate;       //将此旋转变换赋给风机图片控件

            da.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            da.From = 0;
            da.By = 360;

            DependencyProperty[] propertyChain = new DependencyProperty[]
                {
                     UIElement.RenderTransformProperty,
                     RotateTransform.AngleProperty
                };
            Storyboard.SetTargetName(da, control.Name);
            Storyboard.SetTargetProperty(da, new PropertyPath("(0).(1)", propertyChain));
            story.Children.Add(da);
            story.Duration = new Duration(TimeSpan.FromMilliseconds(600));
            story.RepeatBehavior = RepeatBehavior.Forever;

            return new Story(control, story);
            //Task.Run(() =>
            //{
            //    Thread.Sleep(5 * 1000);
            //    //story.Pause(control);// 暂停
            //    //story.Resume(control); // 恢复
            //    //story.Stop(control); // 停止
            //    story.Stop(); // 停止
            //});
        }

    }
}
