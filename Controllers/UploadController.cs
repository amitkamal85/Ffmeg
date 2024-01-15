using NotFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Http.Headers;
using NotFinal.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace Final.Controllers
{
    [Route("Upload")]
    [Authorize]
    public class UploadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        String path = "D:\\Demo\\";
        String convertedPath = "Converted";
        public UploadController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public ActionResult AddOriginal()
        {
            return View();
        }


        [HttpPost]
        [Route("AddVideo")]
        [RequestFormLimits(MultipartBodyLengthLimit = 1500000000)]
        public ActionResult AddVideo(/*IFormFile video*/)
        {
            var file = Request.Form.Files;
            
            long size = 0;
            var mine = ContentDispositionHeaderValue.Parse(file[0].ContentDisposition).FileName.Split(".").Last();
            var filename = String.Concat(Path.GetRandomFileName(), ".mp4");

            //string FilePath = _env.WebRootPath + $@"\{ filename}";

            size += file[0].Length;

            //var mine = video.FileName.Split(".").Last();
            //var fileName = String.Concat(Path.GetRandomFileName(), ".", mine);
            string fname = filename.Replace(@"\", "");
            var savePath = Path.Combine(_env.WebRootPath,"Upload\\", filename);
            try
            {
                 using (var fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    file[0].CopyTo(fileStream);
                }
                var name = filename;
                VideoConfig videoConfig = new VideoConfig()
                {
                    OriginalVideo = name
                };

                _context.VideoConfigs.Add(videoConfig);
                 _context.SaveChanges();
                var id = videoConfig.Id;
                var jsonResult = Json(new { Id = id });

                return Json(id);
            }
            catch (Exception ex)
            {
                return Json(new { Id = 0 }); ;
            }
        }
        [HttpPost]
        [Route("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(int id,string title,string Sypnosis)
        {
            var video = await _context.VideoConfigs.FindAsync(id);
            
            video.Sypnosis = Sypnosis;
            video.Title = title;
            _context.VideoConfigs.Update(video);
            await _context.SaveChangesAsync();
            return Json("Success");
        }

        [HttpPost]
        [Route("Convert240p")]
        public async Task<IActionResult> Convert240p(string id)
        {
            var video= await _context.VideoConfigs.FindAsync(Convert.ToInt32(id));
            
            var convertedvname = video.OriginalVideo/*.Split(".").First()*/;
            var savePath= Path.Combine(_env.WebRootPath, "Upload\\", convertedvname);
            var PathforConvertedFile = path;
            try
            {
                string ffmpegArg = string.Format("-i \"{0}\" -vf scale=352:240 \"{1}\"", savePath, _env.WebRootPath+ "\\Converted\\" + convertedvname + "(240).mp4");
                //string ffmpegArg ="-i "+ savePath + " -vf scale="+size+" "+ convertedvname + "(" + size + ").mp4" + "";
                string ffmpegPath = Path.Combine(_env.ContentRootPath, "ffmPeg", "ffmpeg.exe");
                FFmpegTask ffmpegTask = new FFmpegTask(ffmpegPath, ffmpegArg);
                if(await ffmpegTask.Start())
                {
                    video.LowQuality = convertedvname + "(240).mp4";
                    _context.VideoConfigs.Update(video);
                    await _context.SaveChangesAsync();
                }
                //var size = Getsize(video.LowQuality);

                return Json(video.Id);
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json("Fail");
        }
        [HttpPost]
        [Route("Convert360p")]
        public async Task<IActionResult> Convert360p(string id)
        {
            var video= await _context.VideoConfigs.FindAsync(Convert.ToInt32(id));
            
            var convertedvname = video.OriginalVideo/*.Split(".").First()*/;
            var savePath= Path.Combine(_env.WebRootPath,"Upload\\", convertedvname);
            var PathforConvertedFile = path;
            try
            {
                string ffmpegArg = string.Format("-i \"{0}\" -vf scale=480:360 \"{1}\"", savePath, _env.WebRootPath+ "\\Converted\\" + convertedvname + "(360).mp4");
                //string ffmpegArg ="-i "+ savePath + " -vf scale="+size+" "+ convertedvname + "(" + size + ").mp4" + path
                string ffmpegPath = Path.Combine(_env.ContentRootPath, "ffmPeg", "ffmpeg.exe");
                FFmpegTask ffmpegTask = new FFmpegTask(ffmpegPath, ffmpegArg);
                if (await ffmpegTask.Start())
                {
                   
                    video.MediumQuality =  convertedvname + "(360).mp4";
                    _context.VideoConfigs.Update(video);
                    await _context.SaveChangesAsync();
                    //var size = Getsize(video.MediumQuality);
                    return Json(video.Id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json("Fail");
        }
        [HttpPost]
        [Route("Convert480p")]
        public async Task<IActionResult> Convert480p(string id)
        {
            var video= await _context.VideoConfigs.FindAsync(Convert.ToInt32(id));

            var convertedvname = video.OriginalVideo;/*.Split(".").First();*/
            var savePath= Path.Combine(_env.WebRootPath, "Upload\\", convertedvname);
            var PathforConvertedFile = path;
            try
            {
                string ffmpegArg = string.Format("-i \"{0}\" -vf scale=640:480 \"{1}\"", savePath, _env.WebRootPath+ "\\Converted\\" + convertedvname + "(480).mp4");
                //string ffmpegArg ="-i "+ savePath + " -vf scale="+size+" "+ convertedvname + "(" + size + ").mp4" + "";
                string ffmpegPath = Path.Combine(_env.ContentRootPath, "ffmPeg", "ffmpeg.exe");
                FFmpegTask ffmpegTask = new FFmpegTask(ffmpegPath, ffmpegArg);
                if (await ffmpegTask.Start())
                {
                    video.HighQuality =  convertedvname + "(480).mp4";
                    _context.VideoConfigs.Update(video);
                    await _context.SaveChangesAsync();
                    FileInfo fileInfo=new FileInfo(video.HighQuality);
                    //var size = Getsize(video.HighQuality);
                    return Json(video.Id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json("Fail");
        }
        [HttpPost]
        [Route("Convert720p")]
        public async Task<ActionResult> Convert720p(string id)
        {
            var video= await _context.VideoConfigs.FindAsync(Convert.ToInt32(id));
            
            var convertedvname = video.OriginalVideo/*.Split(".").First()*/;
            var savePath= Path.Combine(_env.WebRootPath, "Upload\\", convertedvname);
            var PathforConvertedFile = path;
            try
            {
                string ffmpegArg = string.Format("-i \"{0}\" -vf scale=1280:720 \"{1}\"", savePath,_env.WebRootPath +"\\Converted\\" + convertedvname + "(720).mp4");
                //string ffmpegArg ="-i "+ savePath + " -vf scale="+size+" "+ convertedvname + "(" + size + ").mp4" + "";
                string ffmpegPath = Path.Combine(_env.ContentRootPath, "ffmPeg", "ffmpeg.exe");
                FFmpegTask ffmpegTask = new FFmpegTask(ffmpegPath, ffmpegArg);
                if (await ffmpegTask.Start())
                {
                    video.VeryHighQuality =  convertedvname + "(720).mp4";
                    _context.VideoConfigs.Update(video);
                    await _context.SaveChangesAsync();
                    //var size=Getsize(video.VeryHighQuality);
                    return Json(video.Id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json("Fail");
        }

        //public ActionResult VideoPlayer(string id)
        //{
        //    if(id==null)
        //    {
        //        return View();
        //    }
        //   var video= _context.VideoConfigs.First(x => x.Id == Convert.ToInt32(id)).OriginalVideo;


        //    return View(video);
        //}

        //public byte[] Getsize(string path)
        //{
        //    return System.IO.File.ReadAllBytes(path);
        //}


    }
    public class FFmpegTask
    {
        public Process process = new Process();

        public FFmpegTask(string ffmpegPath, string arguments)
        {
            process.StartInfo.FileName = ffmpegPath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = false;
        }

        public async Task<bool> Start()
        {
            return process.Start();
        }
    }
}
