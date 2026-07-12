using GitLocalService.Models;
using System.Collections.Generic;

namespace GitLocalService.Services
{
    /// <summary>
    /// 向导服务实现类，管理安装向导的导航和状态
    /// </summary>
    public class WizardService : IWizardService
    {
        /// <summary>
        /// 安装配置对象，存储所有安装选项
        /// </summary>
        public InstallConfig Config { get; } = new InstallConfig();

        /// <summary>
        /// 当前步骤索引，从0开始
        /// </summary>
        public int CurrentStep { get; private set; } = 0;

        /// <summary>
        /// 总步骤数，等于StepNames列表的长度
        /// </summary>
        public int TotalSteps => StepNames.Count;

        /// <summary>
        /// 当前步骤对应的视图名称
        /// </summary>
        public string CurrentViewName => StepNames[CurrentStep];

        /// <summary>
        /// 所有步骤的视图名称列表
        /// <para>按安装流程顺序排列：</para>
        /// <para>0: WelcomeView - 欢迎界面</para>
        /// <para>1: LicenseView - 许可协议</para>
        /// <para>2: DestinationView - 选择安装路径</para>
        /// <para>3: ComponentsView - 选择组件</para>
        /// <para>4: StartMenuView - 开始菜单文件夹</para>
        /// <para>5: EditorView - 默认编辑器</para>
        /// <para>6: BranchView - 初始分支名称</para>
        /// <para>7: PathView - PATH环境变量</para>
        /// <para>8: SshView - SSH可执行文件</para>
        /// <para>9: HttpsView - HTTPS传输后端</para>
        /// <para>10: LineEndingView - 换行符转换</para>
        /// <para>11: TerminalView - 终端模拟器</para>
        /// <para>12: PullView - 默认拉取行为</para>
        /// <para>13: ExtraOptionsView - 额外选项</para>
        /// <para>14: ReadyView - 安装摘要</para>
        /// <para>15: InstallingView - 安装进度</para>
        /// <para>16: FinishedView - 完成界面</para>
        /// </summary>
        public List<string> StepNames { get; } = new List<string>
        {
            "WelcomeView",
            "LicenseView",
            "DestinationView",
            "ComponentsView",
            "StartMenuView",
            "EditorView",
            "BranchView",
            "PathView",
            "SshView",
            "HttpsView",
            "LineEndingView",
            "TerminalView",
            "PullView",
            "ExtraOptionsView",
            "ReadyView",
            "InstallingView",
            "FinishedView"
        };

        /// <summary>
        /// 是否正在安装中
        /// <para>安装过程中禁用导航按钮</para>
        /// </summary>
        public bool IsInstalling { get; set; } = false;

        /// <summary>
        /// 跳转到指定步骤
        /// </summary>
        /// <param name="stepIndex">目标步骤的索引（从0开始）</param>
        /// <remarks>如果stepIndex超出范围，则不执行任何操作</remarks>
        public void GoToStep(int stepIndex)
        {
            if (stepIndex >= 0 && stepIndex < TotalSteps)
            {
                CurrentStep = stepIndex;
            }
        }

        /// <summary>
        /// 前进到下一个步骤
        /// <para>如果当前已是最后一步，则不执行任何操作</para>
        /// </summary>
        public void NextStep()
        {
            if (CurrentStep < TotalSteps - 1)
            {
                CurrentStep++;
            }
        }

        /// <summary>
        /// 后退到上一个步骤
        /// <para>如果当前已是第一步，则不执行任何操作</para>
        /// </summary>
        public void PreviousStep()
        {
            if (CurrentStep > 0)
            {
                CurrentStep--;
            }
        }

        /// <summary>
        /// 判断是否可以前进到下一步
        /// </summary>
        /// <returns>true表示可以前进，false表示已是最后一步</returns>
        public bool CanGoNext()
        {
            return CurrentStep < TotalSteps - 1;
        }

        /// <summary>
        /// 判断是否可以后退到上一步
        /// </summary>
        /// <returns>true表示可以后退，false表示已是第一步或正在安装</returns>
        public bool CanGoPrevious()
        {
            return CurrentStep > 0 && !IsInstalling;
        }

        /// <summary>
        /// 判断是否处于最后一步
        /// </summary>
        /// <returns>true表示当前是最后一步，false表示不是</returns>
        public bool IsLastStep()
        {
            return CurrentStep == TotalSteps - 1;
        }
    }
}