using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.Linq;
using System.Collections.Generic;

using MyStocks.Models;
using MyStocks.ViewModels;

using System.Diagnostics;

namespace MyStocks.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryView : ContentPage
    {
        HistoryViewModel history;
        List<Company> CompaniesSelected;
        String date;
        int numberOfPoints;

        public HistoryView(List<Company> companies, string date, int numberOfPoints)
        {
            InitializeComponent();
            this.CompaniesSelected = companies;
            this.date = date;
            this.numberOfPoints = numberOfPoints;
            BindingContext = this.history = new HistoryViewModel(companies, date);
        }
        
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await history.GetHistory();
            Graph.InvalidateSurface();
        }

        private float getMax()
        {
            float max = 0;

            for (int i = 0; i < history.CompaniesHistory.Count; i++)
            {
                for (int j = 0; j < history.CompaniesHistory[i].results.Count; j++)
                {
                    if (history.CompaniesHistory[i].results[j].close > max)
                        max = history.CompaniesHistory[i].results[j].close;
                }
            }
            return max;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            if (history.Loading)
                return;

            var companies = history.CompaniesHistory.ToList();

            if (companies.Count == 0 || companies == null)
                return;

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            // Clears canvas
            canvas.Clear();

            // Colors   
            SKPaint grayColor = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Gray.ToSKColor(),
                StrokeWidth = 1
            };

            SKPaint lightGrayColor = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.LightGray.ToSKColor(),
                StrokeWidth = 1
            };

            SKPaint yellowColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.FromRgba(240, 236, 213, 180).ToSKColor(),
            };

            SKPaint blueColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.FromRgba(219, 237, 242, 180).ToSKColor()
            };

            SKPaint yellowTrace = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.FromRgba(230, 201, 75, 255).ToSKColor(),
                StrokeWidth = 4
            };

            SKPaint blueTrace = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.FromRgba(45, 195, 214, 255).ToSKColor(),
                StrokeWidth = 4
            };

            SKPaint white = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.FromRgba(255, 255, 255, 0).ToSKColor(),
                StrokeWidth = 1
            };

            SKPaint textColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Gray.ToSKColor(),
                TextSize = 20
            };

            SKPaint smallTextColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Gray.ToSKColor(),
                TextSize = 14
            };

            SKPaint axisColor = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Gray.ToSKColor(),
                TextSize = 10
            };

            List<SKPaint> fillColors = new List<SKPaint>();
            fillColors.Add(yellowColor);
            fillColors.Add(blueColor);

            List<SKPaint> traceColors = new List<SKPaint>();
            traceColors.Add(yellowTrace);
            traceColors.Add(blueTrace);

            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Width (in pixels)
            var width = mainDisplayInfo.Width;

            // Height (in pixels)
            var height = mainDisplayInfo.Height;
            
            var valueSize = getMax();
            var chartWidth = (float)width-100f;
            var chartHeight = (float)height / 2;

            for (int k = 0; k < companies.Count; k++)
            {
                var company = companies[k];

                var invisiblePath = new SKPath();
                using (var path = new SKPath())
                {
                    var minQuote = companies[k].results.OrderBy(x => x.close).First().close;
                    var maxQuote = companies[k].results.OrderByDescending(x => x.close).First().close;
                    var quoteDiff = maxQuote - minQuote;

                    invisiblePath.MoveTo(0, (int)chartHeight);
                    
                    for (int i = 0; i < company.results.Count; i++)
                    {
                        invisiblePath.LineTo(i * chartWidth / (company.results.Count - 1), ((company.results[i].close - minQuote) * chartHeight / quoteDiff));

                        if (i == 0)
                        {
                            path.MoveTo(i * chartWidth / (company.results.Count - 1), ((company.results[i].close - minQuote) * chartHeight / quoteDiff));
                        }

                        else
                        {
                            path.LineTo(i * chartWidth / (company.results.Count - 1), ((company.results[i].close - minQuote) * chartHeight / quoteDiff));
                        }
                    }
                
                    invisiblePath.LineTo(company.results.Count * chartWidth / (company.results.Count), chartHeight);
                    canvas.DrawPath(path, traceColors[k]);
                    canvas.DrawPath(invisiblePath, white);
                    canvas.DrawPath(invisiblePath, fillColors[k]);
                }
            }

            // X axis labels
            for (int i = 0; i < 6; i++)
            {
                canvas.DrawText(companies[0].results[0].timestamp.ToString("MM/dd/yyyy"), i*(chartWidth/(float)6), chartHeight +30, smallTextColor);
            }
            
            // Y axis labels
            for (int i = 0; i < 6; i++)
            {
                canvas.DrawText(Math.Round((i * valueSize) / 5, 2).ToString(), chartWidth+5, (chartHeight * ((5 - i) / (float)5))+10, textColor);
                canvas.DrawLine(0, (chartHeight * ((5 - i) / (float)5)), chartWidth, (chartHeight * ((5 - i) / (float)5)), lightGrayColor);
            }
        }
    }
}