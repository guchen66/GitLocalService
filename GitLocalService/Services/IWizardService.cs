using GitLocalService.Models;
using System.Collections.Generic;

namespace GitLocalService.Services
{
    /// <summary>
    /// 向导服务接口，定义安装向导的导航和状态管理功能
    /// </summary>
    public interface IWizardService
    {
        /// <summary>
        /// 获取当前安装配置对象
        /// <para>包含所有安装步骤中用户选择的配置选项</para>
        /// </summary>
        InstallConfig Config { get; }

        /// <summary>
        /// 获取当前步骤索引
        /// <para>从0开始，对应StepNames列表中的索引</para>
        /// </summary>
        int CurrentStep { get; }

        /// <summary>
        /// 获取总步骤数
        /// <para>等于StepNames列表的长度</para>
        /// </summary>
        int TotalSteps { get; }

        /// <summary>
        /// 获取当前步骤对应的视图名称
        /// <para>用于Prism Region导航</para>
        /// </summary>
        string CurrentViewName { get; }

        /// <summary>
        /// 获取所有步骤的视图名称列表
        /// <para>按顺序排列，每个元素对应一个安装步骤的视图名称</para>
        /// </summary>
        List<string> StepNames { get; }

        /// <summary>
        /// 跳转到指定步骤
        /// </summary>
        /// <param name="stepIndex">目标步骤的索引（从0开始）</param>
        void GoToStep(int stepIndex);

        /// <summary>
        /// 前进到下一个步骤
        /// <para>如果当前已是最后一步，则不执行任何操作</para>
        /// </summary>
        void NextStep();

        /// <summary>
        /// 后退到上一个步骤
        /// <para>如果当前已是第一步，则不执行任何操作</para>
        /// </summary>
        void PreviousStep();

        /// <summary>
        /// 判断是否可以前进到下一步
        /// </summary>
        /// <returns>true表示可以前进，false表示已是最后一步</returns>
        bool CanGoNext();

        /// <summary>
        /// 判断是否可以后退到上一步
        /// </summary>
        /// <returns>true表示可以后退，false表示已是第一步或正在安装</returns>
        bool CanGoPrevious();

        /// <summary>
        /// 判断是否处于最后一步
        /// </summary>
        /// <returns>true表示当前是最后一步，false表示不是</returns>
        bool IsLastStep();

        /// <summary>
        /// 获取或设置是否正在安装中
        /// <para>安装过程中禁用导航按钮</para>
        /// </summary>
        bool IsInstalling { get; set; }
    }
}