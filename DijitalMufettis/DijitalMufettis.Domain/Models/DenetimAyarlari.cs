using DijitalMufettis.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijitalMufettis.Domain.Models
/// <summary>
/// 
///
/// 
///  Bir denetimin ayarları — tüm kuralların paylaştığı ortak bağlam.
/// Şimdilik sadece sektör; ileride posta, dönem, özel durumlar eklenecek.
/// </summary>
/// 
/// 
/// 
/// </summary>
{
    public class DenetimAyarlari
    {
        public required Sektor Sektor { get; init; }    
    }
}
