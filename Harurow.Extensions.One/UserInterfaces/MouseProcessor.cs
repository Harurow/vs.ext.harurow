using System.Windows;
using System.Windows.Input;
using Microsoft.VisualStudio.Text.Editor;

namespace Harurow.Extensions.One.UserInterfaces
{
    internal class MouseProcessor : IMouseProcessor
    {
        private IWpfTextView TextView { get; }

        public MouseProcessor(IWpfTextView textView)
        {
            TextView = textView;
            TextView.Properties.AddProperty(typeof(MouseProcessor), this);
        }

        #region Implements of IMouseProcessor

        /// <inheritdoc />
        public void PreprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseLeftButtonDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseRightButtonDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseRightButtonDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseLeftButtonUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseLeftButtonUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseRightButtonUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseRightButtonUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseUp(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseDown(MouseButtonEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseMove(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseMove(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseWheel(MouseWheelEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseWheel(MouseWheelEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseEnter(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseEnter(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessMouseLeave(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessMouseLeave(MouseEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessDragLeave(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessDragLeave(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessDragOver(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessDragOver(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessDragEnter(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessDragEnter(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessDrop(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessDrop(DragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessQueryContinueDrag(QueryContinueDragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessQueryContinueDrag(QueryContinueDragEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PreprocessGiveFeedback(GiveFeedbackEventArgs e)
        {
        }

        /// <inheritdoc />
        public void PostprocessGiveFeedback(GiveFeedbackEventArgs e)
        {
        }

        #endregion
    }
}