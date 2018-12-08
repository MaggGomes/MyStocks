using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.Reflection;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Diagnostics;


namespace MyStocks.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphView : ContentPage
    {
        /*HistoryViewModel viewModel;
        List<Company> CompaniesSelected;
        String date;

        public GraphView(List<Company> companies, string date)
        {
            InitializeComponent();
            this.CompaniesSelected = companies;
            this.date = date;
            BindingContext = this.viewModel = new HistoryViewModel(companies, date);
            viewModel.stockDetails.CollectionChanged += StockDetails_CollectionChanged;
        }

        private void StockDetails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (viewModel.CanDraw)
            {
                Debug.WriteLine("tamanho teste " + viewModel.stockDetails[0].Count + " lol " + viewModel.CanDraw);
                HistoryGraph.InvalidateSurface();
            }

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Factory.StartNew(() => { viewModel.LoadHistory(); });
        }

        private float getMaxValue()
        {
            float max = 0;
            for (int i = 0; i < viewModel.stockDetails.Count; i++)
            {
                for (int j = 0; j < viewModel.stockDetails[i].Count; j++)
                {
                    if (viewModel.stockDetails[i][j].closeValue > max)
                        max = viewModel.stockDetails[i][j].closeValue;
                }
            }
            return max;
        }

        private float getMinValue()
        {
            float min = 9999999999;
            for (int i = 0; i < viewModel.stockDetails.Count; i++)
            {
                for (int j = 0; j < viewModel.stockDetails[i].Count; j++)
                {
                    if (viewModel.stockDetails[i][j].closeValue < min)
                        min = viewModel.stockDetails[i][j].closeValue;
                }
            }
            return min;
        }*/

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            /*if (!viewModel.CanDraw)
                return;

            // Get Metrics
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            // Width (in pixels)
            var device_width = mainDisplayInfo.Width;
            // Height (in pixels)
            var device_height = mainDisplayInfo.Height;

            var width_ratio = device_width / 720;
            var height_ratio = device_height / 1280;

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            Debug.WriteLine("vou desenhar o canvas");
            Debug.WriteLine("tamanho 1" + viewModel.stockDetails.Count);
            var stockDetails = viewModel.stockDetails.ToList();
            Debug.WriteLine("tamanho 2" + stockDetails.Count);
            if (stockDetails != null)
            {
                Debug.WriteLine("não é nulo");
                SKPaint paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.Gray.ToSKColor(),
                    StrokeWidth = 1
                };

                SKPaint cyanTracePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromRgba(0, 255, 255, 255).ToSKColor(),
                    StrokeWidth = 1
                };

                SKPaint yellowTracePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromRgba(255, 255, 0, 255).ToSKColor(),
                    StrokeWidth = 1
                };

                SKPaint invisiblePaint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = Color.FromRgba(255, 255, 255, 0).ToSKColor(),
                    StrokeWidth = 1
                };

                SKPaint textPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.Black.ToSKColor(),
                    TextSize = 30,
                    TextAlign = SKTextAlign.Center
                };

                SKPaint cyanFillPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.FromRgba(0, 255, 255, 100).ToSKColor(),
                    TextSize = 10,
                    //TextAlign = SKTextAlign.Center
                };

                SKPaint yellowFillPaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.FromRgba(255, 255, 0, 100).ToSKColor(),
                    TextSize = 10,
                    //TextAlign = SKTextAlign.Center
                };

                SKPaint scalePaint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = Color.Gray.ToSKColor(),
                    TextSize = 10,
                    //TextAlign = SKTextAlign.Center
                };

                if (stockDetails.Count <= 0)
                {
                    return;
                }

                List<SKPaint> fillColors = new List<SKPaint>();
                fillColors.Add(cyanFillPaint);
                fillColors.Add(yellowFillPaint);

                List<SKPaint> traceColors = new List<SKPaint>();
                traceColors.Add(cyanTracePaint);
                traceColors.Add(yellowTracePaint);

                var minValue = getMinValue() - 1;
                var maxValue = getMaxValue() + 1;
                var valueDifference = maxValue - minValue;
                var horScale = 30f * (float)width_ratio;
                var vertScale = 200f * (float)height_ratio;
                var realVertScale = vertScale + 10 * (float)height_ratio;

                for (int j = 0; j < 5; j++)
                {
                    canvas.DrawText((minValue + (j * valueDifference) / 4).ToString(), 21f * horScale, (vertScale * ((4 - j) / (float)4)) + 14, scalePaint);
                    canvas.DrawLine(0, (vertScale * ((4 - j) / (float)4)) + 10, 21f * horScale, (vertScale * ((4 - j) / (float)4)) + 10, paint);
                }

                for (int k = 0; k < stockDetails.Count; k++)
                {

                    var stockDetail = stockDetails[k];

                    var invisiblePath = new SKPath();
                    using (var path = new SKPath())
                    {

                        var localMinValue = stockDetails[k].OrderBy(x => x.closeValue).First().closeValue - 1;// hourlyConditions.OrderBy(x => x.Temperature).First().Temperature - 2;
                        var localMaxValue = stockDetails[k].OrderByDescending(x => x.closeValue).First().closeValue + 1;
                        var localValueDifference = localMaxValue - localMinValue;



                        int x0 = 0;//(int)(2 * horScale);
                        int y0 = (int)(realVertScale);//(int)((maxTemp - minTemp + 3) * vertScale);

                        invisiblePath.MoveTo(0, y0);

                        // Draw Horizontal Axis
                        canvas.DrawLine(x0, y0, 21f * horScale, y0, paint);
                        Debug.WriteLine("tamanho " + stockDetail.Count);

                        // Draw Hour Vertical Line
                        canvas.DrawLine(21f * horScale, realVertScale, 21f * horScale, 10, paint);




                        int i;
                        for (i = 0; i < stockDetail.Count; i++)
                        {
                            // Draw Temperature Line 
                            Debug.WriteLine("tou a desenhar " + stockDetail[i].closeValue);
                            if (i == 0)
                            {
                                path.MoveTo(i * (21f * horScale) / (stockDetail.Count - 1), ((stockDetail[i].closeValue - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));
                            }
                            else
                            {
                                path.LineTo(i * (21f * horScale) / (stockDetail.Count - 1), ((stockDetail[i].closeValue - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));
                            }
                            invisiblePath.LineTo(i * (21f * horScale) / (stockDetail.Count - 1), ((stockDetail[i].closeValue - localMinValue) * vertScale / localValueDifference) + (realVertScale - vertScale));

                        }

                        paint.Color = Color.Blue.ToSKColor();
                        paint.StrokeWidth = 2;
                        canvas.DrawPath(path, traceColors[k]);
                        invisiblePath.LineTo(i * (21f * horScale) / (stockDetail.Count), vertScale + (realVertScale - vertScale));
                        canvas.DrawPath(invisiblePath, invisiblePaint);
                        canvas.DrawPath(invisiblePath, fillColors[k]);
                    }
                }

            }*/
        }

        /*void DrawImage(SKCanvas canvas, string icon, float x, float y)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string resourceID = assembly.GetName().Name + "." + icon.Replace("//cdn.apixu.com/", "").Replace("/", ".").Replace("64x64", "_64x64");

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                if (stream != null)
                {
                    using (SKManagedStream skStream = new SKManagedStream(stream))
                    {
                        var resourceBitmap = SKBitmap.Decode(skStream);
                        if (x < 0) x = 0;
                        if (y < 0) y = 0;
                        canvas.DrawBitmap(resourceBitmap, x, y);
                    }
                }
            }
        }*/
    }
}