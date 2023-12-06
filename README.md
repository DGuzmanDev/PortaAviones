# PortaAviones

Repositorio para el desarrollo del proyecto 2 del curso 03101 – Programación avanzada en web de la UNED.

## Los requerimientos mínimos son:

- **Almacenamiento del inventario de aviones.**
  - Controlar la existencia de aviones en el portaviones, nuevos aviones, retirada de aviones, aterrizajes y despegues.
- **Pantalla principal**.
  - Presenta las opciones:
    - Ingreso,
    - Retiro,
    - Listar inventario de aviones
    - Despegue y aterrizaje
- **Ingreso**:
  - Es un proceso para recibir nuevos aviones que se incorporan al portaviones. Cuando es nuevo debe completarse toda la información:
    - Marca (hacer una lista)
    - Modelo del avión (lista filtrada a partir de la marca)
    - Número de serie.
    - Nombre de fantasía (se digita)
    - Dimensiones: ancho de ala a ala, alto, largo de la punta a la cola.
    - Distancia máxima con combustible lleno
    - Al finalizar de llenar los datos, debe existir un botón para guardar los datos. Cuando se agrega, debe aumentar la cantidad de existencia de aviones.
    - Número de orden.
  - Puede agregarse ‘n’ cantidad de aviones nuevos. Hay un botón **“Incluir”**. Al oprimir este botón el avión se incluye en una tabla temporal que muestra cada uno de los aviones que se ha registrado.
  - Debe guardar todos los datos del avión ingresado en una tabla en la que se ira viendo el detalle antes de guardarlo.
  - Hay un botón **“Guardar”**. Al oprimir este botón se aplica el ingreso de los aviones. Se debe guardar el detalle de cada avión debido a que el número de serie se utiliza para buscar.
    - Genera un consecutivo de ingreso, la fecha y hora, el técnico que recibe la información de ingreso.
    - Se debe aumentar la cantidad de inventario en cada tipo de avión basados en el modelo del avión.
- **Retiro**. Se presenta cuando los aviones se han vuelto obsoletos, requiere ser trasladado a otra base, ha sufrido daños que los inhabilita o se perdió durante la misión.
  - Debe solicitarse el número de serie, con ese valor se busca y carga los demás datos en una tabla. Un técnico puede retirar 'n' cantidad de aviones
  - La información del técnico que retira debe registrarse, nombre completo. No hay mantenimiento de personal técnico. Este valor se digita.
  - Existe un botón de limpiar lista llamado **“Limpiar”**. Al oprimir este botón debe presentar un mensaje de advertencia que le pregunta al usuario si desea limpiar los datos, que no se podrían recuperar los datos
  - Existe un botón **“Guardar”**. Al oprimir este botón, se registra el retiro de los aviones de la lista.
  - Se genera un número de consecutivo de retiro con la fecha y el usuario que lo realizó.
  - Un detalle con lo que se retiró.
  - Se aplica el rebajo en el inventario de los aviones según el modelo del avión.
  - No se elimina el registro del avión, solo se descuenta.
- **Listar inventario de aviones.**
  - Hay una pantalla que permite ver la lista de aviones agrupados por modelo en orden alfabético y permitir el cambio de orden:
    - Debe mostrarse el modelo y la cantidad en existencia como primer nivel.
    - En el segundo nivel se muestra la marca, la serie, el nombre de fantasía, la fecha de ingreso y quien lo recibió.
  - No se muestran los aviones retirados.
- **Despegue y aterrizaje:**
  - Hay una pantalla para el registro de despegue y aterrizaje.
  - No afecta los inventarios.
  - **Despegue:** Cada vez que una serie de aviones va a salir a una misión, un técnico debe registrar los despegues, **creando un nuevo encabezado que incluye:**
    - Número de despegue, es autogenerado con un consecutivo en formato: AAAA-DE-CCCCC. Este número se genera hasta que se guarda el despegue.
      - AAAA: Es el año del despegue. Es tomado del servidor de SQL.
      - “DE”: es un valor literal.
      - CCCCC: es un valor consecutivo rellenado con ceros a la izquierda.
    - Fecha y hora que inicia los despegues.
    - Técnico encargado de los despegues.
    - Nombre de la misión.
    - Debe incluir por medio de búsqueda por serie cada uno de los aviones en una lista en la que debe mostrar la marca, modelo y serie del avión y debe capturar el nombre de piloto encargado del avión.
    - Debe permitir eliminar un registro de la lista. Esto por cuanto pude incluirse por error aviones que no van a la misión.
  - Hay un botón **“Limpiar”**.
    - Permite eliminar todos los datos mientras no se hayan guardado. Debe presentar una advertencia al usuario indicando que perderá el trabajo realizado.
  - Hay un botón **“Guardar”**.
    - Registra todos los datos.
    - Genera el consecutivo.
  - **Aterrizaje:**
    - Debe buscar la misión en una lista emergente que permite filtrar por número de despegue. Se carga en una tabla
    - Debe mostrar todos los datos del despegue y capturar:
      - Fecha y hora de retorno de cada avión.
      - Si un avión se perdió durante la misión, debe permitir incluir nueva información:
        - Se marca como perdido en misión.
        - Si hubo pérdidas humanas o si se quiere una misión de rescate.
    - Hay un botón **“Limpiar”**:
      - Si no hay cambios, no muestra advertencia de que se perderá el trabajo.
      - Si hay cambios en algún dato y no se ha guardado, debe mostrar un mensaje de advertencia indicando que perderá su trabajo.
      - Si limpia, la pantalla queda lista para realizar otra nueva búsqueda de despegue o iniciar uno nuevo.
    - Hay un botón **“Guardar”**.
      - Se gurda los datos y se cierra el despegue, por lo que no podrá volverse a modificar.

**Considerar:**

- Cualquier otro supuesto que requiera puede documentarlo, mientras no contradiga los requerimientos del proyecto
- Debe usar servicios WCF o REST para conectar la lógica de negocios/acceso a datos con la interfaz de usuario.
