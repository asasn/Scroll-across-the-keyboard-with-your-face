using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace RootNS.Helper
{
    internal class BinHelper
    {
        /// <summary>
        /// 保存成二进制文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveBin_Click(object sender)
        {
            string filePath = "objPerson.bin";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, sender);
            }
        }

        /// <summary>
        /// 读取二进制文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private object btnReadBin_Click(object sender)
        {
            string filePath = "objPerson.bin";
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return bf.Deserialize(fs);
            }
        }
    }
}
