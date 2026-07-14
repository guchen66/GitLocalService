using GitLocalService.Models;
using GitLocalService.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows;

namespace GitLocalService.ViewModels
{
    /// <summary>
    /// 主窗口视图模型，管理安装向导的整体导航和状态
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// Prism区域管理器，用于导航不同的视图
        /// </summary>
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// 向导服务，管理步骤导航逻辑
        /// </summary>
        private readonly IWizardService _wizardService;

        /// <summary>
        /// 事件聚合器，用于跨ViewModel通信
        /// </summary>
        private readonly IEventAggregator _eventAggregator;

        private readonly ISharedStateService _sharedStateService;

        /// <summary>
        /// 私有字段：窗口标题
        /// </summary>
        private string _title = "Welcome";

        /// <summary>
        /// 窗口标题，显示当前步骤名称
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// 私有字段：是否可以前进到下一步
        /// </summary>
        private bool _canGoNext = true;

        /// <summary>
        /// 是否可以前进到下一步
        /// <para>绑定到Next按钮的IsEnabled属性</para>
        /// </summary>
        public bool CanGoNext
        {
            get => _canGoNext;
            set => SetProperty(ref _canGoNext, value);
        }

        /// <summary>
        /// 私有字段：是否可以后退到上一步
        /// </summary>
        private bool _canGoPrevious = false;

        /// <summary>
        /// 是否可以后退到上一步
        /// <para>绑定到Previous按钮的IsEnabled属性</para>
        /// </summary>
        public bool CanGoPrevious
        {
            get => _canGoPrevious;
            set => SetProperty(ref _canGoPrevious, value);
        }

        /// <summary>
        /// 私有字段：Next按钮的显示文本
        /// </summary>
        private string _nextButtonContent = "Next >";

        /// <summary>
        /// Next按钮的显示文本
        /// <para>根据当前步骤动态变化：</para>
        /// <para>- 普通步骤："Next >"</para>
        /// <para>- 准备安装步骤："Install"</para>
        /// <para>- 安装完成步骤："Finish"</para>
        /// </summary>
        public string NextButtonContent
        {
            get => _nextButtonContent;
            set => SetProperty(ref _nextButtonContent, value);
        }

        /// <summary>
        /// 私有字段：安装进度值（0-100）
        /// </summary>
        private int _progressValue = 0;

        /// <summary>
        /// 安装进度值（0-100）
        /// <para>绑定到进度条控件</para>
        /// <para>非安装阶段：根据步骤计算进度</para>
        /// <para>安装阶段：显示实际安装进度</para>
        /// </summary>
        public int ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        /// <summary>
        /// 私有字段：进度条下方的文本描述
        /// </summary>
        private string _progressText = "";

        /// <summary>
        /// 进度条下方的文本描述
        /// <para>非安装阶段：显示"步骤 X / Y"</para>
        /// <para>安装阶段：显示安装进度描述</para>
        /// </summary>
        public string ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }

        /// <summary>
        /// 私有字段：是否正在安装中
        /// </summary>
        private bool _isInstalling = false;

        /// <summary>
        /// 是否正在安装中
        /// <para>安装过程中禁用导航按钮，显示进度条</para>
        /// </summary>
        public bool IsInstalling
        {
            get => _isInstalling;
            set => SetProperty(ref _isInstalling, value);
        }

        /// <summary>
        /// 后退命令，绑定到Previous按钮
        /// </summary>
        public DelegateCommand PreviousCommand { get; set; }

        /// <summary>
        /// 前进命令，绑定到Next按钮
        /// </summary>
        public DelegateCommand NextCommand { get; set; }

        /// <summary>
        /// 取消命令，绑定到Cancel按钮，关闭应用程序
        /// </summary>
        public DelegateCommand CancelCommand { get; set; }

        private ServiceConfig _serviceConfig;

        /// <summary>
        /// 构造函数，注入依赖服务
        /// </summary>
        /// <param name="regionManager">Prism区域管理器</param>
        /// <param name="wizardService">向导服务</param>
        /// <param name="serviceConfig">服务配置</param>
        /// <param name="eventAggregator">事件聚合器</param>
        public MainWindowViewModel(IRegionManager regionManager, IWizardService wizardService, ServiceConfig serviceConfig, IEventAggregator eventAggregator, ISharedStateService sharedStateService)
        {
            _regionManager = regionManager;
            _wizardService = wizardService;
            _serviceConfig = serviceConfig;
            _eventAggregator = eventAggregator;
            _sharedStateService = sharedStateService;
            // 初始化命令
            PreviousCommand = new DelegateCommand(OnPrevious);
            NextCommand = new DelegateCommand(OnNext);
            CancelCommand = new DelegateCommand(OnCancel);

            // 监听Region集合变化，当ContentRegion创建完成后导航到第一步
            _regionManager.Regions.CollectionChanged += Regions_CollectionChanged;

            // 订阅LicenseAcceptedEvent，当用户在LicenseView中选择时自动更新CanGoNext
            _eventAggregator.GetEvent<LicenseAcceptedEvent>().Subscribe(OnLicenseAcceptedChanged);

            // 订阅SharedStateService的ItemChanged事件，实时响应LicenseView的选择变化
            _sharedStateService.ItemChanged += OnSharedStateChanged;
        }

        /// <summary>
        /// Region集合变化事件处理
        /// <para>当ContentRegion被添加到RegionManager时，自动导航到第一步</para>
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void Regions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var region in e.NewItems)
                {
                    if (region is IRegion r && r.Name == "ContentRegion")
                    {
                        // 导航到第一步（欢迎界面）
                        NavigateToStep(0);
                        // 取消订阅，避免重复触发
                        _regionManager.Regions.CollectionChanged -= Regions_CollectionChanged;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 导航到指定步骤
        /// </summary>
        /// <param name="stepIndex">目标步骤的索引（从0开始）</param>
        private void NavigateToStep(int stepIndex)
        {
            // 更新向导服务中的当前步骤
            _wizardService.GoToStep(stepIndex);

            // 获取ContentRegion并导航到对应的视图
            var region = _regionManager.Regions["ContentRegion"];
            region.RemoveAll();
            region.RequestNavigate(_wizardService.CurrentViewName);

            // 更新按钮状态和标题
            UpdateButtons();
        }

        /// <summary>
        /// 处理后退按钮点击
        /// <para>后退到上一个步骤</para>
        /// </summary>
        private void OnPrevious()
        {
            _wizardService.PreviousStep();
            NavigateToStep(_wizardService.CurrentStep);
        }

        /// <summary>
        /// 处理前进按钮点击
        /// <para>根据当前步骤执行不同的逻辑：</para>
        /// <para>- 步骤14（准备安装）：开始安装，导航到步骤15</para>
        /// <para>- 步骤15（安装中）：安装完成，导航到步骤16</para>
        /// <para>- 其他步骤：前进到下一个步骤</para>
        /// </summary>
        private void OnNext()
        {
            if (_wizardService.CurrentStep == 14)
            {
                // 开始安装
                IsInstalling = true;
                _wizardService.IsInstalling = true;
                NavigateToStep(15);
            }
            else if (_wizardService.CurrentStep == 15)
            {
                // 安装完成，进入完成界面
                NavigateToStep(16);
            }
            else
            {
                // 普通步骤前进
                _wizardService.NextStep();
                NavigateToStep(_wizardService.CurrentStep);
            }
        }

        /// <summary>
        /// 处理取消按钮点击
        /// <para>关闭应用程序</para>
        /// </summary>
        private void OnCancel()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 更新按钮状态和标题
        /// <para>根据当前步骤动态设置：</para>
        /// <para>- 窗口标题</para>
        /// <para>- Next按钮文本</para>
        /// <para>- 按钮可用性</para>
        /// <para>- 进度条和进度文本</para>
        /// </summary>
        private void UpdateButtons()
        {
            // 更新按钮可用性
            CanGoPrevious = _wizardService.CanGoPrevious();
            CanGoNext = !IsInstalling || _wizardService.CurrentStep == 15;

            // 更新进度条（非安装阶段根据步骤计算）
            if (!IsInstalling)
            {
                UpdateStepProgress();
            }

            // 根据当前步骤设置标题和按钮文本
            switch (_wizardService.CurrentStep)
            {
                case 0:
                    Title = "Welcome";
                    NextButtonContent = "Next >";
                    break;

                case 1:
                    Title = "License Agreement";
                    NextButtonContent = "Next >";
                    // CanGoNext = _serviceConfig.AcceptLicense;
                    CanGoNext = _sharedStateService.SelectedItem;
                    break;

                case 2:
                    Title = "Select Destination Location";
                    NextButtonContent = "Next >";
                    break;

                case 3:
                    Title = "Select Components";
                    NextButtonContent = "Next >";
                    break;

                case 4:
                    Title = "Select Start Menu Folder";
                    NextButtonContent = "Next >";
                    break;

                case 5:
                    Title = "Choosing the default editor used by Git";
                    NextButtonContent = "Next >";
                    break;

                case 6:
                    Title = "Adjusting the name of the initial branch";
                    NextButtonContent = "Next >";
                    break;

                case 7:
                    Title = "Adjusting your PATH environment";
                    NextButtonContent = "Next >";
                    break;

                case 8:
                    Title = "Choosing SSH executable";
                    NextButtonContent = "Next >";
                    break;

                case 9:
                    Title = "Choosing HTTPS transport backend";
                    NextButtonContent = "Next >";
                    break;

                case 10:
                    Title = "Configuring the line ending conversions";
                    NextButtonContent = "Next >";
                    break;

                case 11:
                    Title = "Configuring the terminal emulator";
                    NextButtonContent = "Next >";
                    break;

                case 12:
                    Title = "Configuring the default behavior of 'git pull'";
                    NextButtonContent = "Next >";
                    break;

                case 13:
                    Title = "Configuring extra options";
                    NextButtonContent = "Next >";
                    break;

                case 14:
                    Title = "Ready to install";
                    NextButtonContent = "Install";
                    break;

                case 15:
                    Title = "Installing";
                    NextButtonContent = "";
                    CanGoNext = false;
                    break;

                case 16:
                    Title = "Completing the Git Setup Wizard";
                    NextButtonContent = "Finish";
                    CanGoPrevious = false;
                    break;
            }
        }

        /// <summary>
        /// 更新步骤进度
        /// <para>根据当前步骤计算进度条和进度文本</para>
        /// <para>进度条显示：(当前步骤 + 1) / 总步骤 * 100</para>
        /// <para>进度文本显示："步骤 X / Y"</para>
        /// </summary>
        private void UpdateStepProgress()
        {
            int current = _wizardService.CurrentStep + 1;
            int total = _wizardService.TotalSteps;

            ProgressValue = (current * 100) / total;
            ProgressText = $"步骤 {current} / {total}";
        }

        /// <summary>
        /// 当LicenseView中用户选择变化时的事件处理（通过EventAggregator）
        /// <para>自动更新主窗口Next按钮的可用性</para>
        /// </summary>
        /// <param name="isAccepted">是否接受许可协议</param>
        private void OnLicenseAcceptedChanged(bool isAccepted)
        {
            CanGoNext = isAccepted;
        }

        /// <summary>
        /// 当SharedStateService状态变化时的事件处理
        /// <para>通过共享服务实时响应LicenseView的选择变化</para>
        /// </summary>
        /// <param name="newValue">新的选择状态</param>
        private void OnSharedStateChanged(bool newValue)
        {
            CanGoNext = newValue;
        }

        /// <summary>
        /// 更新安装进度
        /// <para>由InstallingViewModel调用，通知主窗口安装进度</para>
        /// </summary>
        /// <param name="value">进度值（0-100）</param>
        /// <param name="text">进度描述文本</param>
        public void UpdateProgress(int value, string text)
        {
            ProgressValue = value;
            ProgressText = text;

            // 如果进度达到100%，表示安装完成
            if (value >= 100)
            {
                IsInstalling = false;
                _wizardService.IsInstalling = false;
                CanGoNext = true;
                NextButtonContent = "Finish";
            }
        }
    }
}