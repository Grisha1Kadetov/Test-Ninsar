using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestNinsar.Enums;
using TestNinsar.ScriptableObjects;
using TestNinsar.Tables;
using TestNinsar.Shapes;
using UnityEngine;
using Input = UnityEngine.Input;
using Random = UnityEngine.Random;

namespace TestNinsar
{

    /// <summary>
    /// Место запуска всего оастального.
    /// </summary>
    public class BootStrap : MonoBehaviour
    {
        [SerializeField] private TextInputType _textInputType;
        [SerializeField, Multiline(10)] private string _input;
        [Header("Links")]
        [SerializeField] private ShapeGridFactory _factory;
        [SerializeField] private CharToShapeDataDictionary _dataDictionary;
        [Space] [SerializeField] private bool _isDebug;

        private NonBoundariesTable<ShapeData> _nonBoundariesTable;
        private AreaOfNonBoundariesTable<ShapeData> _area;

        //Вообще по хорошему нужно сделать ReadOnly версию класса
        //Но ради единстенного применения в Map не хочется заморачиваться
        //Тем более на таком маленьком проекте
        public AreaOfNonBoundariesTable<ShapeData> Area => _area;

        private Shape[,] _shapes;
        private int _x;
        private int _y;
        
        public string CurrentInput { get; private set; }

        public int X => _x;

        public int Y => _y;

        public bool IsBooted { get; private set; }
        
        public Dictionary<char, ShapeData> Dictionary { get; private set; }
        
        public event Action UpdateShapeEvent;
        public event Action BootEvent;
        
        
        #region Main Logic
 
        private void Awake()
        {
            Dictionary = new Dictionary<char, ShapeData>(_dataDictionary.ShapeDictionary);
            
            var input = GetTextInput(_textInputType);
            _shapes = _factory.Build();
            Boot(input);
        }

        //Вообще этот Update можно было бы вынести в отдельный класс вроде WASDInput,
        //но думаю сейчас в этом нет большого смысла в плане архетектуры
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
                _y--;
            if (Input.GetKeyDown(KeyCode.S))
                _y++;
            if (Input.GetKeyDown(KeyCode.D))
                _x++;
            if (Input.GetKeyDown(KeyCode.A))
                _x--;

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                UpdateAllState();
            }
        }
        
        private void Boot(string input)
        {
            CurrentInput = input;
            _nonBoundariesTable = new NonBoundariesTable<ShapeData>
                (new StringToTableConverter<ShapeData>(Dictionary).GetResult(input));
            _area = new AreaOfNonBoundariesTable<ShapeData>
                (_nonBoundariesTable, _factory.SizeX,_factory.SizeY);
            SetRandomPivot();
            UpdateAllState();
            IsBooted = true;
            BootEvent?.Invoke();
        }
        
        private void Reboot()
        {
            _shapes = _factory.Build();
            _area = new AreaOfNonBoundariesTable<ShapeData>
                (_nonBoundariesTable, _factory.SizeX, _factory.SizeY);
            SetRandomPivot();
            UpdateAllState();
            BootEvent?.Invoke();
        }

        private void SetRandomPivot()
        {
            _x = Random.Range(0, _nonBoundariesTable.LengthX);
            _y = Random.Range(0, _nonBoundariesTable.LengthY);
            _area.Pivot = (_x, _y);
        }

        private void UpdateAllState()
        {
            _area.Pivot = (_x, _y);
            (_x, _y) = _nonBoundariesTable.GetNormalizeIndex(_x,_y);
            UpdateShapes();
            
#if UNITY_EDITOR
            if(_isDebug)
                Debug.Log(GetDebug(_area.GetArea()));
#endif
        }
        
        private void UpdateShapes()
        {
            var area = _area.GetArea();
            for(int y = 0; y < _shapes.GetLength(1); y++)
            for (int x = 0; x < _shapes.GetLength(0); x++) 
                _shapes[x, y].ShapeData = area[x, y];
            
            UpdateShapeEvent?.Invoke();
        }

        private string GetTextInput(TextInputType t)
        {
            return t switch
            {
                TextInputType.FromInspector => _input.Trim(),
                TextInputType.FromFile => InputFromFile.GetInput().Trim(),
                TextInputType.Random => 
                    InputRandomizer.GetRandomInput(_dataDictionary.ShapeDictionary.Keys.ToArray(),
                        Random.Range(5, 15), Random.Range(5, 15)).Trim(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
#endregion 
        
#region Public Methods

        public void BootFromFile()
        {
            Boot(GetTextInput(TextInputType.FromFile));
        }
                
        public void BootFromRandom()
        {
            Boot(GetTextInput(TextInputType.Random));
        }
                
        public void BootFromInspector()
        {
            Boot(GetTextInput(TextInputType.FromInspector));
        }
        
        public void IncreaseSizeX()
        {
            _factory.SizeX++;
            Reboot();
        }
        
        public void DecreaseSizeX()
        {
            if (_factory.SizeX <= 1)
                return;
            _factory.SizeX--;
            Reboot();
        }
        public void IncreaseSizeY()
        {
            _factory.SizeY++;
            Reboot();
        }
        
        public void DecreaseSizeY()
        {
            if (_factory.SizeY <= 1)
                return;
            _factory.SizeY--;
            Reboot();
        }
        
#endregion
        
#region Debug
#if UNITY_EDITOR
        private string GetDebug(ShapeData[,] shapeDataTable)
        {
            StringBuilder sb = new StringBuilder("\n");
            for (int j = 0; j < shapeDataTable.GetLength(1); j++)
            {
                for (int i = 0; i < shapeDataTable.GetLength(0); i++)
                    sb.Append(shapeDataTable[i,j].name[0] + " ");
                sb.Append("\n");
            }
            
            return sb.ToString();
        }

        [ContextMenu("Generate Inspector Input")]
        private void GenerateInspectorInput()
        {
            _input = InputRandomizer.GetRandomInput(_dataDictionary.ShapeDictionary.Keys.ToArray(),
                Random.Range(5, 15), Random.Range(5, 15));
        }
        
        [ContextMenu("Generate File Input")]
        private void GenerateFileInput()
        {
           InputFromFile.SetInput(InputRandomizer.GetRandomInput(_dataDictionary.ShapeDictionary.Keys.ToArray(),
                Random.Range(5, 15), Random.Range(5, 15)));
        }
        
        [ContextMenu("Debug File Input")]
        private void LogFileInput()
        {
            Debug.Log(InputFromFile.GetInput());
        }
#endif       
#endregion
    }
}