import React, { useState } from 'react';
import logo from './logo.svg';

function DoubleRead() {
  const [values, setValues] = useState([] as number[])
  return <>
    <h2> Double Read</h2>
    <button type="button" className="btn btn-primary" onClick={() => {
      fetch("/dbTest/doubleRead").then(r => r.json()).then(data => setValues(data))
    }}>Load</button>
    {values.length == 0 ? null : <>
      <h3> Values</h3>
      <ul>
        {values.map((f, idx) => <li key={idx}>{f}</li>)}
      </ul></>
    }
  </>
}
interface TestEntity {
  id: number,
  value: number,
}
function App() {
  const [values, setValues] = useState([] as TestEntity[])
  const [delay, setDelay] = useState(false);
  return (
    <div className="container">
      <h1> TX Demo</h1>
      <button type="button" className="btn btn-primary" onClick={() => {
        fetch("/dbTest").then(r => r.json()).then(data => setValues(data))
      }}>Load</button>
      <button type="button" className="btn btn-primary" onClick={() => {
        fetch(`/dbTest?delay=${delay}`, { method: 'POST' }).then(r => r.json()).then(data => setValues(data))
      }}>Sum and Insert</button>

      <button type="button" className="btn btn-primary" onClick={() => {
        fetch("/dbTest", { method: 'DELETE' }).then(r => r.json()).then(data => setValues(data))
      }}>Clear</button>

      <br />

      <div className="form-check" onClick={() => setDelay(x => !x)}>
        <input className="form-check-input" type="checkbox" checked={delay} />
        <label className="form-check-label">
          Delay
        </label>
      </div>

      {values.length == 0 ? null : <>
        <h3> Values</h3>
        <ul>
          {values.map((f, idx) => <li key={idx}>Id: {f.id} Value: {f.value}</li>)}
        </ul></>
      }
      <DoubleRead />
    </div>
  );
}

export default App;
