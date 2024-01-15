using Microsoft.AspNetCore.Mvc;
using MediaToolkit;
using NotFinal.Areas.Identity.Data;
using NotFinal.Models;
using Microsoft.EntityFrameworkCore;
using MediaToolkit.Model;

namespace NotFinal.Controllers
{
    public class ListingController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        public ListingController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        static readonly string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public string Getsize(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return (((new FileInfo(path).Length)/ (1024 * 1024))+" mb");
            }
            return "mb";
        }

        public async Task<ActionResult> List()
        {
            List<VideoConfig> AllVideos = new List<VideoConfig>();
            var data = await _dbContext.VideoConfigs.ToListAsync();

            foreach (var video in data)
            {
                VideoConfig model = new VideoConfig();
                var filename = video.OriginalVideo;
                var savePath = "/Upload/" + filename;
                model.Id = video.Id;
                model.OriginalVideo = savePath;
                model.Title = video.Title;
                model.CreatedBy = video.CreatedBy;
                model.CreatedDate = video.CreatedDate;
                model.Duration = video.Duration;
                AllVideos.Add(model);
            }
            return View(AllVideos);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteVideoData(string Id)
        {
            var directory = Directory.GetCurrentDirectory();
            var PID = Convert.ToInt32(Id);
            var result = string.Empty;
            if (PID != 0)
            {
                var data = await _dbContext.VideoConfigs.Where(x => x.Id == PID).FirstOrDefaultAsync();
                if (data != null)
                {
                    if (data.LowQuality != null)
                    {
                        var filename = string.Empty;
                        filename = directory + "\\wwwroot\\" + data.LowQuality;
                        //var savePath = Path.Combine(_env.WebRootPath, "Converted\\", filename);

                        System.IO.File.Delete(filename);
                    }
                    if (data.MediumQuality != null)
                    {
                        var filename = string.Empty;
                        filename = directory + "\\wwwroot\\" + data.MediumQuality;
                        //var savePath = Path.Combine(_env.WebRootPath, "Converted\\", filename);
                        System.IO.File.Delete(filename);
                    }
                    if (data.HighQuality != null)
                    {
                        var filename = string.Empty;
                        filename = directory + "\\wwwroot\\" + data.HighQuality;
                        // var savePath = Path.Combine(_env.WebRootPath, "Converted\\", filename);
                        System.IO.File.Delete(filename);
                    }
                    if (data.VeryHighQuality != null)
                    {
                        var filename = string.Empty;
                        filename = directory + "\\wwwroot\\" + data.VeryHighQuality;
                        //var savePath = Path.Combine(_env.WebRootPath, "Converted\\", filename);
                        System.IO.File.Delete(filename);
                    }

                    if (data.OriginalVideo != null)
                    {
                        var filename = string.Empty;
                        filename = directory + "\\wwwroot\\Upload\\" + data.OriginalVideo;
                        //var savePath = Path.Combine(_env.WebRootPath, "Upload\\", filename);
                        System.IO.File.Delete(filename);
                    }

                    _dbContext.Remove(data);
                    await _dbContext.SaveChangesAsync();
                    result = "1";

                }
                else
                {
                    result = "0";
                }
            }
            else
            {
                result = "0";
            }

            return Json(result);
        }
        //public async Task<ActionResult> List()
        //{
        //    List<VideoConfig> AllVideos = new List<VideoConfig>();
        //    var data = await _dbContext.VideoConfigs.ToListAsync();

        //    foreach (var video in data)
        //    {
        //        VideoConfig model = new VideoConfig();
        //        var filename = video.OriginalVideo;
        //        var savePath = "/Upload/" + filename;
        //        model.OriginalVideo = savePath;
        //        model.Id= video.Id;
        //        model.Duration = video.Duration;
        //        model.CreatedDate = video.CreatedDate;
        //        model.CreatedBy = video.CreatedBy;
        //        model.Title = video.Title;
        //        AllVideos.Add(model);
        //    }
        //    return View(AllVideos);
        //}
        public async Task<ActionResult> Display(int id,int? a)
        {
            var display = fileinfo(id,a);
           // var model = _dbContext.VideoConfigs.FirstOrDefault(x=>x.Id==id);

           // Displaysingle display = new Displaysingle();
           // // File Size
           // var file =_env.WebRootPath+"\\Upload\\" + model.OriginalVideo;
           // display.Size = Getsize(file);
           // display.Duration = Timeduration(file);
           // if (a==1)
           // {
           //     file = _env.WebRootPath + "\\"+model.LowQuality;
           //     display.Resolution = "352*240";
           //     display.Size = Getsize(file);
           // }  
           //else if (a==2)
           // {
           //     file = _env.WebRootPath + "\\" + model.MediumQuality;
           //     display.Resolution = "480*360";
           //     display.Size = Getsize(file);
           // } 
           // else if (a==3)
           // {
           //     file = _env.WebRootPath + "\\" + model.HighQuality;
           //     display.Resolution = "640*480";
           //     display.Size = Getsize(file);
           // } 
           // else if (a==4)
           // {
           //     file = _env.WebRootPath + "\\" + model.VeryHighQuality;
           //     display.Resolution = "1280*720";
           //     display.Size = Getsize(file);
           // }

           
            return View(display);
        }
        [HttpPost]
        public Displaysingle fileinfo(int id, int? a)
        {
            var model = _dbContext.VideoConfigs.FirstOrDefault(x => x.Id == id);

            Displaysingle display = new Displaysingle();
            display.Id = model.Id;
            display.Title = model.Title;
            display.Sypnosys = model.Sypnosis;
            // File Size
            var file =  "/Upload/" + model.OriginalVideo;
            var fileactual= _env.WebRootPath + "\\Upload\\" + model.OriginalVideo;
            display.Size = Getsize(fileactual);
            display.Duration = Timeduration(fileactual);
            if (a == 1)
            {
                file = "/Converted/" + model.LowQuality;
                var fileactual1 = _env.WebRootPath + "\\Converted\\" + model.LowQuality;
                display.Resolution = "352*240";
                display.Size = Getsize(fileactual1);
            }
            else if (a == 2)
            {
                file = "/Converted/" + model.MediumQuality;
                var fileactual1 = _env.WebRootPath + "\\Converted\\" + model.MediumQuality;
                display.Resolution = "480*360";
                display.Size = Getsize(fileactual1);
            }
            else if (a == 3)
            {
                file = "/Converted/" + model.HighQuality;
                var fileactual1 = _env.WebRootPath + "\\Converted\\" + model.HighQuality;
                display.Resolution = "640*480";
                display.Size = Getsize(fileactual1);
            }
            else if (a == 4)
            {
                file = "/Converted/" + model.VeryHighQuality;
                var fileactual1 = _env.WebRootPath + "\\Converted\\" + model.VeryHighQuality;
                display.Resolution = "1280*720";
                display.Size = Getsize(fileactual1);
            }
            display.Video = file;

            return display;
        }
        public string Timeduration(string file)
        {
            var inputFile = new MediaFile { Filename = file/*"D:\\Projects\\Kleoui\\Sagar\\Final\\Final\\Final\\wwwroot\\ck0urxb4.kl0.mp4"*/ };

            using (var engine = new Engine())
                engine.GetMetadata(inputFile);
            var durationOfMediaFile = inputFile.Metadata.Duration.TotalMilliseconds;
            TimeSpan t = TimeSpan.FromMilliseconds(durationOfMediaFile);

            String time = String.Empty;
            if (t.Hours < 1)
                time = t.Minutes.ToString() + " minutes " + t.Seconds.ToString() + " Seconds";
            else
                time = t.Hours.ToString() + " hours " + t.Minutes.ToString() + " minutes " + t.Seconds.ToString() + " Seconds";

            return  time;
        }
   

    }
    public class Displaysingle
    {
        public int Id { get; set; }
        public string  Video { get; set; }
        public string Resolution { get; set; }
        public string Size { get; set; }
        public string Duration { get; set; }
        public string Title { get; set; }
        public string Sypnosys { get; set; }

    }
}
