using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Dragging
{
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler where T : class
    {
        // Private states
        Vector3 startPosition;
        Transform originalParent;
        IDragSource<T> source;

        // Cached references
        Canvas parentCanvas;

        //Livecycle methods
        private void Awake()
        {
            parentCanvas = GetComponentInParent<Canvas>();
            source = GetComponentInParent<IDragSource<T>>();
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            startPosition = transform.position;
            originalParent = transform.parent;

            GetComponent<CanvasGroup>().blocksRaycasts = false;
            transform.SetParent(parentCanvas.transform, true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = startPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(originalParent, true);

            IDragDestination<T> container;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                container = parentCanvas.GetComponent<IDragDestination<T>>();
            }
            else
            {
                container = GetContainer(eventData);
            }

            if (container != null)
            {
                DropItemIntoContainer(container);
            }
        }

        private IDragDestination<T> GetContainer(PointerEventData eventData)
        {
            if (eventData.pointerEnter)
            {
                var container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>();
                return container;
            }
            return null;
        }
        private void DropItemIntoContainer(IDragDestination<T> destination)
        {
            if (object.ReferenceEquals(destination, source)) return;

            var destinationContainer = destination as IDragContainer<T>;
            var sourceContainer = source as IDragContainer<T>;

            if (destinationContainer == null || sourceContainer == null ||
                destinationContainer.GetItem() == null || sourceContainer.GetItem() == null)
            {
                AttemptSimpleTransfer(destination);
                return;
            }
            AttempSwamp(destinationContainer, sourceContainer);
        }

        private void AttempSwamp(IDragContainer<T> destination, IDragContainer<T> source)
        {
            var removedSourceNumber = source.GetNumber();
            var removedSourceItem = source.GetItem();
            var removedDestinationNumber = destination.GetNumber();
            var removedDestinationItem = destination.GetItem();

            source.RemoveItems(removedSourceNumber);
            destination.RemoveItems(removedDestinationNumber);

            var sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, source, destination);
            var destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destination, source);

            //Do take back if needed
            if (sourceTakeBackNumber > 0)
            {
                source.AddItems(removedSourceItem, sourceTakeBackNumber);
                removedSourceNumber -= sourceTakeBackNumber;
            }

            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedDestinationItem, destinationTakeBackNumber);
                removedDestinationNumber -= destinationTakeBackNumber;
            }
            //Abort if can't swap 
            if (source.MaxAcceptable(removedDestinationItem) < removedDestinationNumber ||
                destination.MaxAcceptable(removedSourceItem) < removedSourceNumber)
            {
                destination.AddItems(removedDestinationItem, removedDestinationNumber);
                source.AddItems(removedSourceItem, removedSourceNumber);
                return;
            }

            // swap
            if (removedDestinationNumber > 0)
            {
                source.AddItems(removedDestinationItem, removedDestinationNumber);
            }
            if (removedSourceNumber > 0)
            {
                destination.AddItems(removedSourceItem, removedSourceNumber);
            }
        }
        private bool AttemptSimpleTransfer(IDragDestination<T> destination)
        {
            var draggingItem = source.GetItem();
            var draggingNumber = source.GetNumber();

            var acceptable = destination.MaxAcceptable(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer > 0)
            {
                source.RemoveItems(toTransfer);
                destination.AddItems(draggingItem, toTransfer);
                return false;
            }

            return true;
        }
        private int CalculateTakeBack(T removedItem, int removedNumber, IDragContainer<T> takenFrom, IDragContainer<T> takenTo)
        {
            var takeBackNumber = 0;
            var destinationMaxAcceptable = takenTo.MaxAcceptable(removedItem);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = takenFrom.MaxAcceptable(removedItem);

                if (sourceTakeBackAcceptable < takeBackNumber) return 0;

            }


            return takeBackNumber;
        }
    }

}
