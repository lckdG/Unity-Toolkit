# Unity-BehaviorTree
Unity-BehaviorTree is a simple extension I wrote as a base system for running AIs with Behavior Tree. It comes with an Editor Window.

## Credit
This work is based on a tutorial from [this tutorial](https://youtu.be/nKpM98I7PeM?si=-UtoN0EHa4JseNAJ) -- [TheKiwiCoder](https://www.youtube.com/@TheKiwiCoder).

## Usage
- **Create Behavior Tree** asset from **Assets > Create > AI > Behavior Tree**. Each tree will be created with a Blackboard asset.
- Attach a **Behavior Tree Runner** to the GameObject, and drag the tree asset to *Clone From* field. Upon 
- To **edit** the tree: double click on the tree asset to open the editor window. It also creates a *Root* node on first open.
- Node types: **Composite**, **Decorator** and **Action**.
- Inherit the base nodes for further customization, based on design needs.

## The Behavior Tree
- The behavior tree consists of two components: **the tree itself** and **the blackboard**. 
- The tree is a graph with nodes, starting from *Root*. It is a representation of the logical flow. Attach more nodes to define the AI in the game environment.
- The Blackboard is the data house for the tree. Each node will retrieves and writes data into the blackboard.
- From the **Behavior Tree Editor**, or directly clicking on the **Blackboard** asset. There will be the context field, displaying the blackboard inner data. The nodes can also write new data point, refer as *BlackboardKeyMapping* to the Blackboard.

## The Behavior Tree Runner
- The main class for manipulating the Behavior Tree. When a GameObject is instantiated, the Behavior Tree Runner will *clone* the reference Behavior Tree is referencing for use.
- To truly run the tree, call ***UpdateTree***. This should be called from a class that directly communicate with the Runner.
- The Behavior Tree Runner is in responsibility for:
    - Setting the **"target"**. Basically, the **target** is the GameObject that contains the communicating class.
    - Changing the blackboard data, which should only be done sparsely.
    - Resetting the tree to its initial state.

## Tree Traversal
- Based on the [Visitor pattern](https://en.wikipedia.org/wiki/Visitor_pattern). Used for traversing the whole tree and perform an action, e.g. resetting the node's states.
- To create custom tree traversal action: inherit the **INodeVisitor** interface.
- The derivatives of *INodeVisitor* will be called directly by the **BehaviorTree**.

## Miscellaneous
- BehaviorConstants.cs: to store Blackboard key constants, to gracefully reference the keys in multiple locations without using string literals.
- BehaviorTreeUtility.cs: to keep the repeatedly used functions at one place.