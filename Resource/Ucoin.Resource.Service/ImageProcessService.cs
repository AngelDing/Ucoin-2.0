using System;
using System.Collections.Generic;
using Ucoin.Resource.Entity;
using Ucoin.Resource.IService;

namespace Ucoin.Resource.Service
{
    public class ImageProcessService : IImageProcessService
    {
        private Lazy<ImageManager> imgManager = new Lazy<ImageManager>(() =>
        {
            return new ImageManager();
        }, true);

        public ImageReturnEntity ImgUpload(ImageParamEntity param)
        {
            return imgManager.Value.ImageUpload(param);
        }

        public List<ImageReturnEntity> ImgUploadList(List<ImageParamEntity> param)
        {
            var list = new List<ImageReturnEntity>();
            param.ForEach(c =>
            {
                var result = imgManager.Value.ImageUpload(c);
                if (result.IsComplete)
                {
                    list.Add(result);
                }
                else
                {
                    throw new Exception(string.Format("{0}", result.ReturnMessage));
                }
            });
            return list;
        }

        public ImageReturnEntity ImgDel(ImageParamEntity param)
        {
            throw new NotImplementedException();
        }
    }
}
