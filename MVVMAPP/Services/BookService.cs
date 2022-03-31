using MVVMAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAPP.Services
{
    public class BookService
    {
        private Book CurBook = new Book();

        public BookService(Book currentBook)
        {
            CurBook = currentBook;
        }

        public void LoadToTree0(object parameter)
        {
            if (CurBook.RootNodes[0].Count == 0)
            {
                CurBook.Name = "测试书籍";
                CurBook.RootNodes[0].Add(new Node("新分卷1"));
                CurBook.RootNodes[0].Add(new Node("新分卷2"));
                CurBook.RootNodes[0].Add(new Node("新分卷3"));

                CurBook.RootNodes[0][0].ChildNodes.Add(new Node("新章节1"));
                CurBook.RootNodes[0][0].ChildNodes.Add(new Node("新章节2"));
                CurBook.RootNodes[0][0].ChildNodes.Add(new Node("新章节3"));
                CurBook.RootNodes[0][1].ChildNodes.Add(new Node("新章节1"));
                CurBook.RootNodes[0][1].ChildNodes.Add(new Node("新章节2"));
                CurBook.RootNodes[0][2].ChildNodes.Add(new Node("新章节1"));
            }
        }



        public void LoadToTree1(object parameter)
        {
            if (CurBook.RootNodes[1].Count == 0)
            {
                CurBook.Name = "测试书籍";
                CurBook.RootNodes[1].Add(new Node("新分卷1"));

                CurBook.RootNodes[1][0].ChildNodes.Add(new Node("新章节1"));
            }
        }


        public void LoadToTree2(object parameter)
        {
            if (CurBook.RootNodes[2].Count == 0)
            {
                CurBook.Name = "测试书籍";
                CurBook.RootNodes[2].Add(new Node("新分卷1"));
                CurBook.RootNodes[2].Add(new Node("新分卷2"));
            }
        }
    }
}
