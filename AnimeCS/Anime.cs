/*
 * Copyright 2018 Alejandro Cardenas
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished
 * to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *  
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using AnimeCS.AnimationProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
namespace AnimeCS {
    /// <author>Alejandro Cardenas Campos</author>
    /// <summary>
    /// A class to create animations whit ease.
    /// </summary>
    public class Anime {

        public const int TIME = 250;

        public bool isRunning { get; private set; }
        public bool isPaused { get; private set; }

        private int currentAnimation;
        private List<Storyboard> animations;
        private FrameworkElement[] targets;
        private Action<Object> animationsCompleted;
        private Action animationsChanged;


        public Anime(AnimeProperties properties) {
            animations = new List<Storyboard>();
            targets = properties.getTargets();
            animations.Add(createStoryboard(properties));
        }

        public Anime then(AnimeProperties properties) {
            properties.targets = this.targets;
            animations.Add(createStoryboard(properties));
            return this;
        }

        public Anime completed(Action<Object> completed) {
            this.animationsCompleted = completed;
            return this;
        }

        public Anime changed(Action changed) {
            this.animationsChanged = changed;
            return this;
        }

        public void start() {
            if (!isRunning) {
                currentAnimation = 0;
                isRunning = true;
                animations[currentAnimation].Begin(targets.ElementAt(0),true);
            } else if (isPaused) {
                animations[currentAnimation].Resume(targets.ElementAt(0));
                isPaused = false;
            }
        }

        public void pause() {
            if (isRunning) {
                animations[currentAnimation].Pause(targets.ElementAt(0));
                isPaused = true;
            }
        }

        private void animationCompleted(object sender,EventArgs e) {
            currentAnimation++;
            if (currentAnimation < animations.Count) {
                animations[currentAnimation].Begin(targets.ElementAt(0),true);
            } else {
                isRunning = false;
                animationsCompleted?.Invoke(sender);
            }
        }


        private Storyboard createStoryboard(IEnumerable<FrameworkElement> elements,int time,IEnumerable<Tuple<String,object>> properties) {
            Storyboard sb = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromMilliseconds(time));

            foreach (var el in elements) {
                foreach (var prop in properties) {
                    Timeline animation = createAnimation(duration,prop.Item2);
                    Storyboard.SetTargetProperty(animation,new PropertyPath(prop.Item1));
                    Storyboard.SetTarget(animation,el);
                    sb.Children.Add(animation);
                }
            }

            sb.Completed += animationCompleted;
            sb.Changed += (s,e) => {
                animationsChanged?.Invoke();
            };
            return sb;
        }

        private Storyboard createStoryboard(AnimeProperties properties) {
            Storyboard sb = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromMilliseconds(properties.time));

            foreach (var el in properties.getTargets()) {
                foreach (var prop in properties.getAnimeProperties()) {
                    if (properties.easing != null && prop.easing == null) {
                        prop.easing = properties.easing;
                    }

                    Timeline animation = prop.getTimeLine(el,duration);

                    //if(properties.repeat != null)
                    //    animation.RepeatBehavior = properties.repeat;

                    foreach (var target in prop.Targets) {
                        Storyboard.SetTargetProperty(animation,target);
                    }
                    Storyboard.SetTarget(animation,el);

                    sb.Children.Add(animation);
                }
            }
            if (properties.delay > 0)
                sb.BeginTime = TimeSpan.FromMilliseconds(properties.delay);
            sb.Completed += animationCompleted;
            sb.Changed += (s,e) => {
                animationsChanged?.Invoke();
            };
            return sb;
        }

        public Timeline createAnimation(Duration duration,object value) {
            Timeline tl = null;

            if (value is byte) {
                tl = new ByteAnimation((byte)value,duration);
            } else if (value is Int16) {
                tl = new Int16Animation((Int16)value,duration);
            } else if (value is Int32) {
                tl = new Int32Animation((Int32)value,duration);
            } else if (value is Int64) {
                tl = new Int64Animation((Int64)value,duration);
            } else if (value is double) {
                tl = new DoubleAnimation((double)value,duration);
            } else if (value is decimal) {
                tl = new DecimalAnimation((decimal)value,duration);
            } else if (value is Thickness) {
                tl = new ThicknessAnimation((Thickness)value,duration);
            } else if (value is SolidColorBrush) {
                tl = new ColorAnimation((value as SolidColorBrush).Color,duration);
            }

            return tl;
        }
    }
}
