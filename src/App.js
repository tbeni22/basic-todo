import './App.css';
import {ListView} from "./components/ListView";

function App() {
    return (
    <div className="App">
        <h1>Tasks to do</h1>
        <ListView num={2}/>
    </div>
  );
}

export default App;
