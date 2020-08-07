﻿using Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Svg2Xaml
{
    public class Converter
    {
        public static string ConvertSVG(string filename, int size = 24)
        {
            var doc = Svg.SvgDocument.Open(filename);

            Viewbox vb = new Viewbox()
            {
                Height = 24,
                Width = 24
            };
            Canvas canvas = new Canvas()
            {
                Height = doc.Height,
                Width = doc.Width,
            };

            List<FrameworkElement> elements = new List<FrameworkElement>();
            GetAllElements(doc, ref elements);
            for (int i = 0; i < elements.Count; i++)
            {
                canvas.Children.Add(elements[i]);
            }
            vb.Child = canvas;

            var xaml = XamlWriter.Save(vb);

            string key = System.IO.Path.GetFileNameWithoutExtension(filename);
            key = key.Replace('-', '_');

            string template = $"<ControlTemplate x:Key=\"{key}\" >" + xaml + "</ControlTemplate>";
            return template;
        }

        public static List<FrameworkElement> GetAllElements(SvgElement svg, ref List<FrameworkElement> previous)
        {
            for (int i = 0; i < svg.Children.Count; i++)
            {
                if (svg.Children[i].GetType() == typeof(SvgGroup))
                {
                    GetAllElements(svg.Children[i], ref previous);
                }
                else if (svg.Children[i].GetType() == typeof(SvgFragment))
                {
                    GetAllElements(svg.Children[i], ref previous);
                }
                else if (svg.Children[i].GetType() == typeof(Svg.Document_Structure.SvgSymbol))
                {
                    GetAllElements(svg.Children[i], ref previous);
                }
                else if (svg.Children[i].GetType() == typeof(SvgElementCollection))
                {
                    GetAllElements(svg.Children[i], ref previous);
                }
                else
                {
                    var el = GetElement(svg.Children[i]);
                    if (el != null)
                    {
                        previous.Add(GetElement(svg.Children[i]));
                    }
                }
            }
            return previous;
        }


        public static Ellipse GetCircle(SvgCircle circle)
        {
            Ellipse cir = new Ellipse()
            {
                Fill = GetBrush(circle),
                StrokeThickness = circle.StrokeWidth,
                Width = circle.Radius * 2,
                Height = circle.Radius * 2,
            };

            Canvas.SetLeft(cir, circle.CenterX - circle.Radius);
            Canvas.SetTop(cir, circle.CenterY - circle.Radius);
            return cir;
        }

        public static FrameworkElement GetElement(SvgElement svg)
        {
            if (svg.GetType() == typeof(SvgPath))
            {
                return GetPath((SvgPath)svg);
            }
            else if (svg.GetType() == typeof(SvgRectangle))
            {
                return GetRectangle((SvgRectangle)svg);
            }
            else if (svg.GetType() == typeof(SvgCircle))
            {
                return GetCircle((SvgCircle)svg);
            }
            else
            {
                //throw new NotImplementedException("");
                return null;
            }
        }

        public static Rectangle GetRectangle(SvgRectangle rect)
        {
            Rectangle rectangle = new Rectangle()
            {
                Height = rect.Height,
                Width = rect.Width,
                Fill = GetBrush(rect),
            };
            Canvas.SetLeft(rectangle, rect.Location.X);
            Canvas.SetTop(rectangle, rect.Location.Y);
            return rectangle;
        }

        public static SolidColorBrush GetBrush(SvgVisualElement svg)
        {
            var color = svg.Fill.GetBrush(svg, SvgRenderer.FromNull(), svg.Fill.Opacity);
            var c = ((System.Drawing.SolidBrush)color).Color;
            return new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
        }

        public static Path GetPath(SvgPath path)
        {
            string data = path.PathData.ToString();
            Path p = new Path()
            {
                Data = Geometry.Parse(data),
                Fill = GetBrush(path),
                StrokeThickness = path.StrokeWidth.Value,
            };
            return p;
        }

        public static string BuildResourceDictionary(string[] filenames)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < filenames.Length; i++)
            {
                builder.Append(ConvertSVG(filenames[i]) + "\n");
            }
            return "<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\""+
                " xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">\n" + builder.ToString() + "</ResourceDictionary>";
        }
        public class LocatedShape : FrameworkElement
        {
            public double Left;
            public double Top;
            public Shape Shape;
        }
    }
}