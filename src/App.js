import './App.css';
import {ListView} from "./components/ListView";
import {EditView} from "./components/EditView";
import {Divider} from "@mui/material";

function App() {
    return (
    <div className="App">
        <div id="top-panel">
            <h1>Tasks to do</h1>
            <EditView/>
            <Divider variant="middle"/>
        </div>
        <ListView num={2}/>
    </div>
  );
}

export default App;
