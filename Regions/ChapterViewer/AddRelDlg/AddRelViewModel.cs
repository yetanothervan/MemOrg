using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
using MemOrg.Interfaces;
using Microsoft.Practices.ServiceLocation;

namespace ChapterViewer.AddRelDlg
{
    public class AddRelViewModel : ViewModelBase
    {
        public AddRelViewModel()
        {
            RelTypes = RelationRepository.RelationTypes.Select(rt => rt.Caption).ToList();
        }

        private IRelationRepository _relationRepository;
        private IRelationRepository RelationRepository
        {
            get
            {
                if (_relationRepository != null)
                    return _relationRepository;

                _relationRepository =
                    (IRelationRepository)
                        ServiceLocator.Current.GetService(typeof(IRelationRepository));
                return _relationRepository;
            }
        }

        public List<string> RelTypes { get; set; }
        public string RelType { get; set; }
    }
}
