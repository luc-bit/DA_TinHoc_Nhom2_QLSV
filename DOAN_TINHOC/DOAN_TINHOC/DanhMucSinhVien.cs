using DOAN_TINHOC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace DA_tinhoc
{
    [Serializable]
    internal class DanhMucSinhVien
    {
        private List<SinhVien> m_dsSinhVien;

        public List<SinhVien> DsSinhVien
        {
            get { return m_dsSinhVien; }
            set { m_dsSinhVien = value; }
        }

        public DanhMucSinhVien()
        {
            m_dsSinhVien = new List<SinhVien>();
        }
        public DanhMucSinhVien(List<SinhVien> dssinhvien)
        {
            m_dsSinhVien = dssinhvien; ;
        }
        public bool KienTraMa(string ma)
        {
            foreach (SinhVien sv in m_dsSinhVien)
            {
                if (sv.MaSV.Equals(ma))
                    return true;
            }
            return false;
        }
        public bool Them(SinhVien sv)
        {
            if (KienTraMa(sv.MaSV))
            {
                return false;
            }
            else
            {
                m_dsSinhVien.Add(sv);
                return true;
            }
        }
        public bool Xoa(SinhVien sv, int vitri)
        {
            if (KienTraMa(sv.MaSV))
            {
                m_dsSinhVien.RemoveAt(vitri);
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool Sua(SinhVien sv, int vitri)
        {
            if (KienTraMa(sv.MaSV))
            {
                m_dsSinhVien[vitri] = sv;
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
