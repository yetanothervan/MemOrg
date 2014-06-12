using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ChapterViewer.AddRelDlg;
using ChapterViewer.BlockPointOutDlg;
using DAL.Entity;
using MemOrg.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.ServiceLocation;
using Block = DAL.Entity.Block;

namespace ChapterViewer
{
    public class ContentViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        public ContentViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<PageSelected>().Subscribe(OnPageSelected);
            _eventAggregator.GetEvent<ParticleChanged>().Subscribe(OnParticleChanged);
            _eventAggregator.GetEvent<BlockChanged>().Subscribe(OnBlockChanged);
            EditCommand = new DelegateCommand(Edit, () => (CurrentParagpaph != null && CurrentParagpaph.Editible));
            SaveCommand = new DelegateCommand(Save, () => TextChanged);
            DiscardCommand = new DelegateCommand(Discard, () => TextChanged);
            CloseEditingCommand = new DelegateCommand(CloseEditing);
            AddSourceCommand = new DelegateCommand(AddSource, () => _curPage != null && _curPage.IsBlockSource);
            ToBlockCommand = new DelegateCommand(ToBlock, () => _paragraphSelection != null && _curPage.IsBlockSource);
            ToRelCommand = new DelegateCommand(ToRel, () => _curPage != null && _curPage.IsBlockSource);
            DeleteCommand = new DelegateCommand(Delete, () => (CurrentParagpaph != null && CurrentParagpaph.Editible));
            BlockNavigateCommand = new DelegateCommand<Block>(BlockNavigate);
            
            EditWindowVisible = Visibility.Collapsed;
        }

        private void OnBlockChanged(Block obj)
        {
            if (_curPage != null && _curPage.Block.BlockId == obj.BlockId)
                _curPage.Block = obj;
            BuildDoc();
        }
        
        private void OnParticleChanged(Particle obj)
        {
            var source = obj as SourceTextParticle;
            var user = obj as UserTextParticle;
            if (source == null && user == null) return;
            
            var part =
                _particleParagraphs
                    .First(p => p.MyParticle != null && p.MyParticle.ParticleId == obj.ParticleId);
            
            part.SetText(obj);
        }

        private IPage _curPage;
        private void OnPageSelected(IPage curPage)
        {
            if (DialogAwaits != null)
            {
                HandleDialogAwaits(curPage.Block);
                return;
            }

            _curPage = curPage;
            Discard();
            CloseEditing();
            CurrentParagpaph = null;
            
            BuildDoc();
        }

        private void HandleDialogAwaits(Block targetBlock)
        {
            if (DialogAwaits is BlockPointOutView)
            {
                var dlg = DialogAwaits as BlockPointOutView;
                dlg.MyBlock = targetBlock;
                dlg.ShowDialog();

                if (dlg.Result == null || dlg.Result == BlockPointOutViewResult.Cancel)
                {
                    DialogAwaits = null; return;
                }
                if (dlg.Result == BlockPointOutViewResult.SelectBlock) return;

                if (dlg.IsCreateNew)
                {
                    DialogAwaits = null;
                    ManagementService.ExtractNewBlockFromParticle(dlg.MyParticle,
                        dlg.StartSelection, dlg.SelectionLength, dlg.Caption);
                }
                else
                {
                    DialogAwaits = null;
                    ManagementService.ExtractParticleToExistBlock(dlg.MyParticle, dlg.MyBlock, 
                        dlg.StartSelection, dlg.SelectionLength);
                }
            }

            if (DialogAwaits is AddRelView)
            {
                var dlg = DialogAwaits as AddRelView;
                if (dlg.Result == AddRelDlgResult.SelectFirst)
                    dlg.BlockFirst = targetBlock;
                else
                    dlg.BlockSecond = targetBlock;
                
                dlg.ShowDialog();
                if (dlg.Result == null || dlg.Result == AddRelDlgResult.Cancel)
                {
                    DialogAwaits = null;
                    return;
                }
                if (dlg.Result == AddRelDlgResult.OK)
                {
                    ManagementService.AddNewRelation(dlg.RelType, 
                        dlg.IsFirstCreateNew ? null : dlg.BlockFirst, dlg.CaptionFirst,
                        dlg.IsSecondCreateNew ? null : dlg.BlockSecond, dlg.CaptionSecond,
                        (dlg.MyParticle != null) ? dlg.MyParticle.MyParticle : null, 
                        dlg.StartSelection, dlg.SelectionLength);
                    DialogAwaits = null;
                    return;
                }

                DialogAwaits = dlg; //select first or select second
            }
        }

        private void BuildDoc()
        {
            var doc = new FlowDocument();
            _particleParagraphs = new List<ParticleParagraph>();
            
            //caption
            doc.Blocks.Add(new Paragraph(new Hyperlink(new Run(_curPage.Block.Caption))
                        {
                            Command = BlockNavigateCommand,
                            CommandParameter = _curPage.Block
                        }));

            //table
            var table = new Table();
            table.Columns.Add(new TableColumn {Width = new GridLength(5, GridUnitType.Star)});
            table.Columns.Add(new TableColumn {Width = new GridLength(1, GridUnitType.Star)});

            var tableRowGroup = new TableRowGroup();
            table.RowGroups.Add(tableRowGroup);

            foreach (var part in _curPage.Block.Particles.OrderBy(b => b.Order))
            {
                var tr = new TableRow();
                tr.Cells.Add(new TableCell(CreateParagraph(part)));
                
                var myPar = _curPage.MyParagraphs.FirstOrDefault(p => p.ParticleId == part.ParticleId);
                if (myPar != null)
                {
                    var np = new Paragraph();
                    foreach (var usedInBlock in myPar.UsedInBlocks)
                    {
                        var hl = new Hyperlink(new Run(usedInBlock.Caption))
                        {
                            Command = BlockNavigateCommand,
                            CommandParameter = usedInBlock
                        };
                        np.Inlines.Add(hl);
                        np.Inlines.Add(new LineBreak());
                    }
                    tr.Cells.Add(new TableCell(np));
                }

                tableRowGroup.Rows.Add(tr);
            }

            doc.Blocks.Add(table);

            Document = doc;
            
            SetParagraphSelection(null);
            AddSourceCommand.RaiseCanExecuteChanged();
            ToBlockCommand.RaiseCanExecuteChanged();
            ToRelCommand.RaiseCanExecuteChanged();
        }

        private List<ParticleParagraph> _particleParagraphs;
        private ParticleParagraph CreateParagraph(Particle p)
        {
            var pp = _curPage.MyParagraphs.FirstOrDefault(m => m.ParticleId == p.ParticleId);

            var paragraph = new ParticleParagraph(p, 
                pp != null ? pp.ParagraphType : ParagraphType.SourceNoQuotes);

            paragraph.MouseDown += (sender, args) =>
            {
                CurrentParagpaph = paragraph;
            };

            _particleParagraphs.Add(paragraph);
            return paragraph;
        }

        public DelegateCommand<Block> BlockNavigateCommand { get; set; }
        private void BlockNavigate(Block block)
        {
            _eventAggregator.GetEvent<BlockNavigated>().Publish(block);
        }

        public DelegateCommand EditCommand { get; set; }
        private void Edit()
        {
            EditWindowVisible = Visibility.Visible;
        }

        public DelegateCommand SaveCommand { get; set; }
        private void Save()
        {
            if (CurrentParagpaph == null || CurrentParagpaph.MyParticle == null) return;
            ManagementService.UpdateParticleText(CurrentParagpaph.MyParticle.ParticleId, SelectedParagpaphText);
            TextChanged = false;
        }

        public DelegateCommand DiscardCommand { get; set; }
        private void Discard()
        {
            RaisePropertyChangedEvent("SelectedParagpaphText");
            TextChanged = false;
        }
        
        public DelegateCommand CloseEditingCommand { get; set; }
        private void CloseEditing()
        {
            EditWindowVisible = Visibility.Collapsed;
        }

        public DelegateCommand AddSourceCommand { get; set; }
        private void AddSource()
        {
            if (_curPage != null && _curPage.Block != null && _curPage.IsBlockSource)
                ManagementService.AddSourceParticle(_curPage.Block);

            var maxOrderParagraph =
               _particleParagraphs.Where(p => p.MyParticle != null)
                    .Aggregate((curmax, x) =>
                        (curmax == null || x.MyParticle.Order > curmax.MyParticle.Order) ? x : curmax);
            
            CurrentParagpaph = maxOrderParagraph;
        }

        public DelegateCommand DeleteCommand { get; set; }
        private void Delete()
        {
            if (CurrentParagpaph != null && CurrentParagpaph.MyParticle != null)
                ManagementService.RemoveSourceParticle(CurrentParagpaph.MyParticle);
        }

        public Window OwnerWindow { get; set; }

        public DelegateCommand ToBlockCommand { get; set; }
        private void ToBlock()
        {
            var par = _paragraphSelection.Start.Paragraph as ParticleParagraph;
            if (par != null)
            {
                var dlg = new BlockPointOutView(par.MyParticle,
                    GetSelectionStart(par), GetSelectionLength())
                {Owner = OwnerWindow};

                dlg.ShowDialog();
                if (dlg.Result == null || dlg.Result == BlockPointOutViewResult.Cancel) return;
                
                if (dlg.Result == BlockPointOutViewResult.Ok && dlg.IsCreateNew)
                {
                    ManagementService.ExtractNewBlockFromParticle(dlg.MyParticle,
                        dlg.StartSelection, dlg.SelectionLength, dlg.Caption);
                    return;
                }
                
                DialogAwaits = dlg; //select block
            }
        }

        private int GetSelectionLength()
        {
            var e = _paragraphSelection.Start.GetOffsetToPosition(_paragraphSelection.End);
            return e;
        }

        private int GetSelectionStart(ParticleParagraph par)
        {
            var s = par.ContentStart.GetOffsetToPosition(_paragraphSelection.Start) - 1;
            return s;
        }

        public DelegateCommand ToRelCommand { get; set; }

        private void ToRel()
        {
            var selected = false;
            var selectStart = 0;
            var selectLength = 0;
            ParticleParagraph par = null;
            if (_paragraphSelection != null)
            {
                par = _paragraphSelection.Start.Paragraph as ParticleParagraph;
                if (par != null)
                {
                    selected = true;
                    selectStart = GetSelectionStart(par);
                    selectLength = GetSelectionLength();
                }
            }

            var dlg = new AddRelView {Owner = OwnerWindow};
            if (selected)
            {
                dlg.Selection = true;
                dlg.StartSelection = selectStart;
                dlg.SelectionLength = selectLength;
                dlg.MyParticle = par;
            }

            dlg.ShowDialog();
            if (dlg.Result == null || dlg.Result == AddRelDlgResult.Cancel) return;
            if (dlg.Result == AddRelDlgResult.OK)
            {
                ManagementService.AddNewRelation(dlg.RelType, null, dlg.CaptionFirst, null, dlg.CaptionSecond, 
                    dlg.MyParticle.MyParticle, dlg.StartSelection, dlg.SelectionLength);
                return;
            }

            DialogAwaits = dlg; //select first or select second
        }

        public void ParagraphBlur()
        {
            foreach (var b in _particleParagraphs)
            {
                b.IsSelected = false;
            }
        }

        private ParticleParagraph _currentParagpaph;
        public ParticleParagraph CurrentParagpaph
        {
            get { return _currentParagpaph; }
            set
            {
                if (Equals(_currentParagpaph, value))
                    return;
                
                ParagraphBlur();

                SelectedParagpaphText = null;

                if (value != null)
                {
                    value.IsSelected = true;
                    SelectedParagpaphText = new TextRange(value.ContentStart, value.ContentEnd).Text;
                }

                _currentParagpaph = value;
                RaisePropertyChangedEvent("CurrentParagpaph");
                EditCommand.RaiseCanExecuteChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private string _particleEditText;
        public string SelectedParagpaphText
        {
            get
            {
                return _particleEditText;
            }
            set
            {
                _particleEditText = value;
                RaisePropertyChangedEvent("SelectedParagpaphText");
            }
        }

        private FlowDocument _document;
        public FlowDocument Document
        {
            get { return _document; }
            set
            {
                _document = value;
                RaisePropertyChangedEvent("Document");
            }
        }

        private bool _textChanged;
        public bool TextChanged
        {
            get { return _textChanged; }
            set
            {
                _textChanged = value;
                SaveCommand.RaiseCanExecuteChanged();
                DiscardCommand.RaiseCanExecuteChanged();
            }
        }

        private IGraphManagementService _managementService;
        private IGraphManagementService ManagementService
        {
            get
            {
                if (_managementService != null)
                    return _managementService;

                _managementService =
                    (IGraphManagementService)
                        ServiceLocator.Current.GetService(typeof(IGraphManagementService));
                return _managementService;
            }
        }

        private Visibility _editWindowVisible;
        public Visibility EditWindowVisible
        {
            get
            {
                return _editWindowVisible;
            }
            set
            {
                _editWindowVisible = value;
                TextChanged = false;
                RaisePropertyChangedEvent("EditWindowVisible");
            }
        }

        private TextSelection _paragraphSelection;

        private object DialogAwaits;
        
        public void SetParagraphSelection(TextSelection selection)
        {
            _paragraphSelection = selection;
            if (selection != null)
            {
                var par = selection.Start.Paragraph as ParticleParagraph;
                if (par != null && par.MyParticle == null)
                    _paragraphSelection = null;
            }
            ToBlockCommand.RaiseCanExecuteChanged();
        }
    }
}
