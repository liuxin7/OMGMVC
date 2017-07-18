using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCcc.Models;

namespace MVCcc.Controllers
{
    public class UserController : Controller
    {
        private MVCccContext db = new MVCccContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            var userList = from user in db.UserModels
                           orderby user.Name descending
                           select user;
            //userList = db.UserModels.OrderBy(u => u.Message).ThenBy(c=>c.Name);
            //userList.Take(5);   //返回制定的数据  
            //userList.Skip(10);   //  取超过10条后数据
            return View(userList.ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            UserModels usermodels = db.UserModels.Find(id);
            if (usermodels == null)
            {
                return HttpNotFound();
            }
            return View(usermodels);
        }

        //
        // GET: /User/Create
        /// <summary>
        /// 需要展示 的添加  界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create
        /// <summary>
        /// 添加的 动作
        /// </summary>
        /// <param name="usermodels"> view 取到的 数据   只要view 中字段的 与 模型中字段是一致的   就可以直接获取到</param>
        /// <returns></returns>
        [HttpPost] //  这个只会接受post 请求   改成 get 的话 就会接受 get 请求
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModels usermodels)
        {
            if (ModelState.IsValid) //  如果验证 通过的 话 就会添加
            {
                db.UserModels.Add(usermodels); //  把数据给 实体
                db.SaveChanges();             // 执行添加      
                return RedirectToAction("Index");  // 跳转到 主页
            }

            return View(usermodels);   //  没有通过的话   直接把元数据 返回   就不需要在重新 填写 数据   只修改不正确的数据
        }

        //
        // GET: /User/Edit/5
        /// <summary>
        /// 修改的界面 
        /// </summary>
        /// <param name="id">主键ID   这个ID 要和view中传的 跳转 传的参数ID   是一致的   -- 这个应该不可能  默认为0   </param>
        /// <returns></returns>
        public ActionResult Edit(int id = 0)
        {
            UserModels usermodels = db.UserModels.Find(id);  //  用主键 去上下文中的实体中查  
            if (usermodels == null)
            {
                return HttpNotFound();
            }
            return View(usermodels);    //  不为空  返回查到的数据
        }

        //
        // POST: /User/Edit/5
        /// <summary>
        ///  修改的 动作 
        /// </summary>
        /// <param name="usermodels"> 实体     --如果view 中的 字段 与实体字段是一致的   就会把界面的数据 传到实体 </param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModels usermodels)
        {
            if (ModelState.IsValid)   // 检查数据验证 是否通过
            {
                db.Entry(usermodels).State = EntityState.Modified;   //  实体对象的 状态
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usermodels);   // 没有通过  返回原数据      直接改 验证不通过的 数据    
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserModels usermodels = db.UserModels.Find(id);
            if (usermodels == null)
            {
                return HttpNotFound();
            }
            return View(usermodels);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserModels usermodels = db.UserModels.Find(id);
            db.UserModels.Remove(usermodels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}