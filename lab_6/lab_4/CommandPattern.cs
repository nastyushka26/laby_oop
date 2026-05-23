// CommandPattern.cs
using System;
using System.Collections.Generic;

namespace laba2oop
{
    /// <summary>
    /// Command Pattern - Encapsulates a request as an object,
    /// allowing parameterization and queuing of requests.
    /// Used for Undo/Redo functionality.
    /// </summary>

    // Command interface
    public interface ICommand
    {
        void Execute();
        void Undo();
        string Description { get; }
    }

    // Concrete command: AddShapeCommand
    public class AddShapeCommand : ICommand
    {
        private ShapeList _shapeList;
        private Shape _shape;
        private int _shapeIndex;

        public AddShapeCommand(ShapeList shapeList, Shape shape)
        {
            _shapeList = shapeList;
            _shape = shape;
            _shapeIndex = -1;
        }

        public string Description => $"Added {_shape.GetType().Name}";

        public void Execute()
        {
            _shapeList.Add(_shape);
            _shapeIndex = _shapeList.ArrayShapes.Count - 1;
        }

        public void Undo()
        {
            if (_shapeIndex >= 0 && _shapeIndex < _shapeList.ArrayShapes.Count)
            {
                _shapeList.ArrayShapes.RemoveAt(_shapeIndex);
                _shapeList.CountShapes--;
            }
        }
    }

    // Concrete command: RemoveShapeCommand
    public class RemoveShapeCommand : ICommand
    {
        private ShapeList _shapeList;
        private Shape _shape;
        private int _shapeIndex;

        public RemoveShapeCommand(ShapeList shapeList, int index)
        {
            _shapeList = shapeList;
            _shapeIndex = index;
            if (index >= 0 && index < _shapeList.ArrayShapes.Count)
            {
                _shape = _shapeList.ArrayShapes[index];
            }
        }

        public string Description => $"Removed {_shape?.GetType().Name ?? "Shape"}";

        public void Execute()
        {
            if (_shapeIndex >= 0 && _shapeIndex < _shapeList.ArrayShapes.Count)
            {
                _shape = _shapeList.ArrayShapes[_shapeIndex];
                _shapeList.ArrayShapes.RemoveAt(_shapeIndex);
                _shapeList.CountShapes--;
            }
        }

        public void Undo()
        {
            if (_shape != null)
            {
                _shapeList.ArrayShapes.Insert(_shapeIndex, _shape);
                _shapeList.CountShapes++;
            }
        }
    }

    // ClearAllCommand
    public class ClearAllCommand : ICommand
    {
        private ShapeList _shapeList;
        private List<Shape> _backupShapes;

        public ClearAllCommand(ShapeList shapeList)
        {
            _shapeList = shapeList;
            _backupShapes = new List<Shape>();
        }

        public string Description => "Cleared all shapes";

        public void Execute()
        {
            _backupShapes.Clear();
            _backupShapes.AddRange(_shapeList.ArrayShapes);
            _shapeList.ArrayShapes.Clear();
            _shapeList.CountShapes = 0;
        }

        public void Undo()
        {
            _shapeList.ArrayShapes.Clear();
            _shapeList.ArrayShapes.AddRange(_backupShapes);
            _shapeList.CountShapes = _backupShapes.Count;
        }
    }

    /// <summary>
    /// Invoker for Command Pattern - manages command history
    /// </summary>
    public class CommandManager
    {
        private Stack<ICommand> _undoStack = new Stack<ICommand>();
        private Stack<ICommand> _redoStack = new Stack<ICommand>();

        public event EventHandler<string>? CommandExecuted;
        public event EventHandler? UndoRedoStateChanged;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();

            CommandExecuted?.Invoke(this, command.Description);
            UndoRedoStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);

                CommandExecuted?.Invoke(this, $"Undo: {command.Description}");
                UndoRedoStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);

                CommandExecuted?.Invoke(this, $"Redo: {command.Description}");
                UndoRedoStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            UndoRedoStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}